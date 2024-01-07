using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Cards.Empire.Units
{
    public class TieBomber : EmpireGalaxyUnit, IHasAbility
    {
        public TieBomber(int id, SWDBGame game) :
            base(id, 2, 2, 0, 0, "Tie Bomber", new List<Trait>{ Trait.fighter }, false, game) {}


        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Game.PendingActions.Add(PendingAction.Of(Action.DiscardCardFromCenter));
        }

        public override int GetTargetValue() 
        {
            return 2;
        }

        public override void ApplyReward() 
        {
            AddExilePendingAction(Game.Rebel, 1);
        }
    }
}