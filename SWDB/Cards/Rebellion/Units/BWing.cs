using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Cards.Rebellion.Units
{
    public class BWing : RebelGalaxyUnit, IHasAbility
    {
        private bool hasAttackBoost;
        public BWing(int id, SWDBGame game) :
            base(id, 5, 5, 0, 0, "B-Wing", new List<Trait>{ Trait.fighter }, false, game)
        {
            hasAttackBoost = false;
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            hasAttackBoost = true;
            if (Owner?.Opponent?.Hand.Any() ?? false) 
            {
                Game.PendingActions.Add(PendingAction.Of(Action.BWingDiscard, () => hasAttackBoost = false, true));
            }
        }

        public override int GetTargetValue() 
        {
            return 5;
        }

        public override void ApplyReward() 
        {
            AddExilePendingAction(Game.Empire, 2);
        }

        public override void MoveToDiscard() 
        {
            base.MoveToDiscard();
            hasAttackBoost = false;
        }

        public override void MoveToInPlay() 
        {
            base.MoveToInPlay();
            hasAttackBoost = false;
        }

        public override int Attack 
        {
            get
            {
                return hasAttackBoost ? base.Attack + 2 : base.Attack;
            }
        }
    }
}