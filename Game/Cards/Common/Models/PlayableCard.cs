using Game.Cards.Common.Models.Interface;
using Game.Common.Interfaces;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Common.Models
{
    public class PlayableCard : Card, IPlayableCard
    {
        public int Cost { get; protected set; }
        public virtual int Attack { get; protected set; }
        public virtual int Resources { get; protected set; }
        public virtual int Force { get; protected set; }
        public IReadOnlyCollection<Trait> Traits { get; protected set; }
        protected bool CanAttack { get; set; } = true;

        protected PlayableCard(int id, Faction faction, string title, bool isUnique, CardLocation location,
            IList<ICard> cardList, SWDBGame game, IPlayer? owner, int cost, int attack, int resources,
            int force, IReadOnlyCollection<Trait> traits) : base(id, faction, title, isUnique, location, cardList, game, owner)
        {
            Cost = cost;
            Attack = attack;
            Resources = resources;
            Force = force;
            Traits = traits;
        }

        public bool AbleToAttack() 
        {
            return Attack > 0 && CanAttack;
        }

        public void SetAttacked() 
        {
            CanAttack = false;
        }

        public void Buy(IPlayer newOwner) 
        {
            Owner = newOwner;
            MoveToDiscard();
        }

        public void BuyToTopOfDeck(IPlayer newOwner) 
        {
            Owner = newOwner;
            MoveToTopOfDeck();
        }

        public void BuyToHand(IPlayer newOwner) 
        {
            Owner = newOwner;
            MoveToHand();
        }

        public virtual void MoveToDiscard() 
        {
            if (Owner == null) 
            {
                throw new ArgumentException("Can not move a card into play with no owner");
            }
            CardList?.Remove(this);
            Owner.Discard.Insert(0, this);
            CardList = Owner.Discard;
            CanAttack = false;
            Location = CardLocationHelper.GetDiscard(Owner.Faction);
            AbilityUsed = false;
        }

        public void MoveToTopOfDeck() 
        {
            if (Owner == null) 
            {
                throw new ArgumentException("Can not move a card into play with no owner");
            }
            CardList?.Remove(this);
            Owner.Deck.Insert(0, this);
            CardList = Owner.Deck;
            Location = CardLocationHelper.GetDeck(Owner.Faction);
        }

        public virtual void MoveToHand() 
        {
            if (Owner == null) 
            {
                throw new ArgumentException("Can not move a card into play with no owner");
            }
            CardList?.Remove(this);
            Owner.Hand.Add(this);
            CardList = Owner.Hand;
            Location = CardLocationHelper.GetHand(Owner.Faction);
        }

        public void MoveToExile() 
        {
            if (Owner == null || Owner.Game == null) 
            {
                throw new ArgumentException("Can not move a card into play with no owner");
            }
            CardList?.Remove(this);
            Owner.Game.ExiledCards.Add(this);
            Owner = null;
            Location = CardLocation.Exile;
            CardList = Game.ExiledCards;
        }

        public virtual void MoveToInPlay() 
        {
            CanAttack = true;
            if (Owner == null || Owner.Game == null) 
            {
                throw new ArgumentException("Can not move a card into play with no owner");
            }
            CardList?.Remove(this);
            Owner.AddResources(Resources);
            if (Owner.Faction == Faction.empire) 
            {
                Owner.Game.ForceBalance.DarkSideGainForce(Force);
            } else 
            {
                Owner.Game.ForceBalance.LightSideGainForce(Force);
            }
        }

        public void MoveToGalaxyDiscard() 
        {
            CanAttack = false;
            Owner = null;
            CardList?.Remove(this);
            Location = CardLocation.GalaxyDiscard;
            Game.GalaxyDiscard.Insert(0, this);
            CardList = Game.GalaxyDiscard;
        }

        public void MoveToGalaxyRow() 
        {
            CanAttack = false;
            Owner = null;
            CardList?.Remove(this);
            Location = CardLocation.GalaxyRow;
            Game.GalaxyRow.Add(this);
            CardList = Game.GalaxyRow;
        }

        public void MoveToTopOfGalaxyDeck() 
        {
            CanAttack = false;
            Owner = null;
            CardList?.Remove(this);
            Location = CardLocation.GalaxyDeck;
            Game.GalaxyDeck.Insert(0, this);
            CardList = Game.GalaxyDeck;
        }

        protected bool IsInPlay() 
        {
            return Owner!= null &&
                    (Location == CardLocationHelper.GetUnitsInPlay(Owner.Faction) ||
                            Location == CardLocationHelper.GetShipsInPlay(Owner.Faction));
        }

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && IsInPlay();
        }

        public override string ToString() 
        {
            return "PlayableCard{" +
                    "id=" + Id +
                    ", title='" + Title + '\'' +
                    ", owner=" + (Owner != null ? Owner.Faction : "none") +
                    ", canAttack=" + CanAttack +
                    '}';
        }
    }
}