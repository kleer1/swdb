using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Game.Cards.Rebellion.Units
{
    public class DurosSpy : RebelGalaxyUnit, IHasAbility
    {
        public DurosSpy(int id, SWDBGame game) :
            base(id, 2, 0, 2, 0, "Duros Spy", new List<Trait>{ Trait.trooper }, false, game) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && 
                ((Owner?.Opponent?.Hand.Any() ?? false) || (!Owner?.DoesPlayerHaveFullForce() ?? false));
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            if (!Owner?.Opponent?.Hand.Any() ?? false) 
            {
                Owner?.AddForce(1);
            } else if (Owner?.DoesPlayerHaveFullForce() ?? false) 
            {
                Game.PendingActions.Add(PendingAction.Of(Action.DiscardFromHand, true));
            } else 
            {
                Owner?.AddForce(1);
                Game.PendingActions.Add(PendingAction.Of(Action.DurosDiscard, () => Owner?.AddForce(-1), true));
            }
        }

        public override int GetTargetValue() 
        {
            return 2;
        }

        public override void ApplyReward() 
        {
            AddExilePendingAction(Game.Empire, 1);
        }
    }
}