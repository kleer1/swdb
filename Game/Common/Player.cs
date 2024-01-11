using Game.Cards.Common.Models.Interface;
using Game.Common.Interfaces;
using SWDB.Game.Utils;
using static SWDB.Game.Utils.ListExtension;

namespace SWDB.Game.Common
{
    public class Player : IPlayer
    {
        public Faction Faction { get; private set; }
        public CastedList<ICard, IPlayableCard> Hand { get; set; } = new  CastedList<ICard, IPlayableCard>();
        public CastedList<ICard, IPlayableCard> Deck { get; set; } = new  CastedList<ICard, IPlayableCard>();
        public CastedList<ICard, IPlayableCard> Discard { get; set; } = new  CastedList<ICard, IPlayableCard>();
        public IBase? CurrentBase { get; set; }
        public CastedList<ICard, IBase> AvailableBases { get; set; } = new CastedList<ICard, IBase>();
        public CastedList<ICard, IBase> DestroyedBases { get; set; } = new CastedList<ICard, IBase>();
        public CastedList<ICard, IUnit> UnitsInPlay { get; set; } = new CastedList<ICard, IUnit>();
        public CastedList<ICard, ICapitalShip> ShipsInPlay { get; set; } = new CastedList<ICard, ICapitalShip>();
        public int Resources { get; set; } = 0;
        public IPlayer? Opponent { get; set; }
        public SWDBGame? Game { get; set; }

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
                    Discard = new CastedList<ICard, IPlayableCard>();
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
            foreach (IUnit card in UnitsInPlay) {
                if (card.AbleToAttack()) attack += card.Attack;
            }
            foreach (ICapitalShip card in ShipsInPlay) {
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

        private static void DiscardList<T>(IList<T> list) where T : IPlayableCard
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