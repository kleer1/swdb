using SWDB.Cards.Common.Models;
using SWDB.Common;

namespace SWDB.Game
{
    public class Player
    {
        public Faction Faction { get; private set; }
        public IList<PlayableCard> Hand { get; } = new List<PlayableCard>();
        public IList<PlayableCard> Deck { get; private set; } = new List<PlayableCard>();
        public IList<PlayableCard> Discard { get; private set; } = new List<PlayableCard>();
        public Base? CurrentBase { get; set; }
        public IList<Base> AvailableBases { get; } = new List<Base>();
        public IList<Base> DestroyedBases { get; } = new List<Base>();
        public IList<Unit> UnitsInPlay { get; } = new List<Unit>();
        public IList<CapitalShip> ShipsInPlay { get; internal set; } = new List<CapitalShip>();
        public int Resources { get; private set; } = 0;
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
                    Discard = new List<PlayableCard>();
                    Deck = Deck.OrderBy(x => Random.Shared.Next()).ToList();
                }
                Deck.First().MoveToHand();
            }
        }

        public void DiscardUnits() 
        {
            DiscardList((IList<PlayableCard>) UnitsInPlay);
        }

        public void DiscardHand() 
        {
            DiscardList(Hand);
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

        private static void DiscardList(IList<PlayableCard> list)
        {
            for (int i = list.Count - 1; i >= 0; i--) 
            {
                PlayableCard card = list[i];
                list.RemoveAt(i);
                card.MoveToDiscard();
            }
        }
    }
}