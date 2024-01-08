using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Cards.Rebellion.Units
{
    public class Snowspeeder : RebelGalaxyUnit, IHasAbility
    {
        public Snowspeeder(int id, SWDBGame game) :
            base(id, 2, 2, 0, 0, "Snowspeeder", new List<Trait>{ Trait.vehicle }, false, game) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && (Owner?.Opponent?.Hand.Any() ?? false);
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Game.PendingActions.Add(PendingAction.Of(Action.DiscardFromHand, true));
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