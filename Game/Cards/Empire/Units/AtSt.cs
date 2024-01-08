using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Game.Cards.Empire.Units
{
    public class AtSt : EmpireGalaxyUnit, IHasAbility
    {
        public AtSt(int id, SWDBGame game) :
            base(id, 4, 4, 0, 0, "AT-ST", new List<Trait>{ Trait.vehicle }, false, game) {}

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Game.PendingActions.Add(PendingAction.Of(Action.DiscardCardFromCenter));
        }

        public override int GetTargetValue() 
        {
            return 4;
        }

        public override void ApplyReward() 
        {
            AddExilePendingAction(Game.Empire, 2);
        }
    }
}