using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Utils;
using static SWDB.Game.Utils.ListExtension;

namespace SWDB.Game.Common
{
    public class Player
    {
        public Faction Faction { get; private set; }
        public CastedList<Card, PlayableCard> Hand { get; } = new  CastedList<Card, PlayableCard>();
        public CastedList<Card, PlayableCard> Deck { get; private set; } = new  CastedList<Card, PlayableCard>();
        public CastedList<Card, PlayableCard> Discard { get; private set; } = new  CastedList<Card, PlayableCard>();
        public Base? CurrentBase { get; set; }
        public CastedList<Card, Base> AvailableBases { get; } = new CastedList<Card, Base>();
        public CastedList<Card, Base> DestroyedBases { get; } = new CastedList<Card, Base>();
        public CastedList<Card, Unit> UnitsInPlay { get; } = new CastedList<Card, Unit>();
        public CastedList<Card, CapitalShip> ShipsInPlay { get; internal set; } = new CastedList<Card, CapitalShip>();
        public int Resources { get; internal set; } = 0;
        public Player? Opponent { get; internal set; }
        public SWDBGame? Game { get; internal set; }

        public Player(Faction faction) 
        {
            Faction = faction;
        }

        public void AddResources(int amount) 
        {
            Resources += amount;
            if (Resources < 0) {
                Resources = 0;
            }
        }

        public void DrawCards(int amount) 
        {
            if (amount > Deck.Count + Discard.Count) 
            {
                amount = Deck.Count + Discard.Count;
            }

            if (amount < 1) 
            {
                return;
            }

            for (int i = 0; i < amount; i++) 
            {
                if (Deck.Count == 0) 
                {
                    Deck = Discard;
                    Discard = new CastedList<Card, PlayableCard>();
                    Deck.Shuffle();
                }
                Deck.BaseList.First().MoveToHand();
            }
        }

        public void DiscardUnits() 
        {
            DiscardList(UnitsInPlay.BaseList);
        }

        public void DiscardHand() 
        {
            DiscardList(Hand.BaseList);
        }

        public int GetAvailableAttack() 
        {
            int attack = 0;
            foreach (Unit card in UnitsInPlay) {
                if (card.AbleToAttack()) attack += card.Attack;
            }
            foreach (CapitalShip card in ShipsInPlay) {
                if (card.AbleToAttack()) attack += card.Attack;
            }
            return attack;
        }

        public void AddForce(int amount) 
        {
            if (Faction == Faction.empire) {
                Game?.ForceBalance.DarkSideGainForce(amount);
            } else {
                Game?.ForceBalance.LightSideGainForce(amount);
            }
        }

        public bool IsForceWithPlayer() 
        {
            if (Faction == Faction.empire) 
            {
                return Game?.ForceBalance.DarkSideHasTheForce() ?? false;
            } else 
            {
                return Game?.ForceBalance.LightSideHasTheForce() ?? false;
            }
        }

        public bool DoesPlayerHaveFullForce() 
        {
            if (Game == null) return false;
            
            if (Faction == Faction.empire) {
                return Game.ForceBalance.DarkSideFull();
            } else {
                return Game.ForceBalance.LightSideFull();
            }
        }

        public override string ToString() 
        {
            return "Player{" +
                    "faction=" + Faction +
                    "\n\thand=" + Hand +
                    "\n\tcurrentBase=" + CurrentBase +
                    "\n\tshipsInPlay=" + ShipsInPlay +
                    "\n\tunitsInPlay=" + UnitsInPlay +
                    "\n\tdestroyedBases=" + DestroyedBases.Count +
                    ", resources=" + Resources +
                    "\n\tgalaxyRow=" + Game?.GalaxyRow +
                    "\n}";
        }

        private static void DiscardList<T>(IList<T> list) where T : PlayableCard
        {
            for (int i = list.Count - 1; i >= 0; i--) 
            {
                T card = list[i];
                list.RemoveAt(i);
                card.MoveToDiscard();
            }
        }
    }
}