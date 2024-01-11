using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;
using Game.Cards.Common.Models.Interface;
using Game.Common.Interfaces;

namespace SWDB.Game.Cards.Common.Models
{
    public class Card : ICard
    {
        public int Id { get; private set; }
        public Faction Faction { get; private set; }
        public string Title { get; private set; }
        public bool IsUnique { get; private set; }
        public IPlayer? Owner { get; set; }
        public SWDBGame Game { get; private set; }
        public CardLocation Location { get; set; }
        public IList<ICard>? CardList { get; set; }
        protected bool AbilityUsed { get; set; }

        protected Card (int id, Faction faction, string title, bool isUnique, CardLocation location, 
            IList<ICard>? cardList, SWDBGame game, IPlayer? owner)
        {
            Id = id;
            Faction = faction;
            Title = title;
            IsUnique = isUnique;
            Game = game;
            Location = location;
            CardList = cardList;
            CardList?.Add(this);
            Owner = owner;
            AbilityUsed = false;
        }

        public virtual bool AbilityActive() 
        {
            return !AbilityUsed && this is IHasAbility;
        }

        public virtual void ApplyAbility() 
        {
            AbilityUsed = true;
        }

        public override bool Equals(Object? obj)
        {
             //Check for null and compare run-time types.
            if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
            {
                return false;
            } else 
            {
                Card card = (Card) obj;
                return Id == card.Id;
            }
        }

        public override int GetHashCode() => HashCode.Combine(Id);

        private int MaxNumExileAbility(IPlayer player) 
        {
            if (player == null) 
            {
                return 0;
            }
            return player.Hand.Count + player.Discard.Count;
        }

        protected void AddExilePendingAction(IPlayer player, int depth) 
        {
            depth = Math.Min(depth, MaxNumExileAbility(player));
            if (depth < 1) 
            {
                return;
            }
            Game.PendingActions.Add(ExileActionRec(depth));
        }

        private PendingAction ExileActionRec(int depth) 
        {
            if (depth == 1) 
            {
                return PendingAction.Of(Action.ExileCard);
            }
            return PendingAction.Of(Action.ExileCard, () => Game.PendingActions.Add(ExileActionRec(depth - 1)));
        }
    }
}