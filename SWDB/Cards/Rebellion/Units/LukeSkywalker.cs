using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Cards.Rebellion.Units
{
    public class LukeSkywalker : RebelGalaxyUnit, IHasAbility
    {
        public LukeSkywalker(int id, SWDBGame game) :
            base(id, 8, 6, 0, 2, "Luke Skywalker", new List<Trait>{ Trait.jedi }, true, game) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && 
                (Owner?.IsForceWithPlayer() ?? false) && 
                (Owner?.Opponent?.ShipsInPlay.Any() ?? false);
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Game.PendingActions.Add(PendingAction.Of(Action.LukeDestroyShip));
        }

        public override int GetTargetValue() 
        {
            return 8;
        }

        public override void ApplyReward() 
        {
            Game.Empire.AddResources(4);
            Game.Empire.AddForce(4);
        }
    }
}