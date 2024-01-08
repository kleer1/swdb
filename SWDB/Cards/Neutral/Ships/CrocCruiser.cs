using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Cards.Neutral.Ships
{
    public class CrocCruiser : NeutralGalaxyShip, IHasAbility
    {
        public CrocCruiser(int id, SWDBGame game) :
            base(id, 3, 0, 1, "C-ROC Cruiser", game, 3) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && Owner != null && Owner.Hand.Any() && Owner.CurrentBase != null &&
                    Owner.CurrentBase.CurrentDamage > 0;
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Game.PendingActions.Add(PendingAction.Of(Action.DiscardFromHand, () => Owner?.CurrentBase?.AddDamage(-3)));
        }
    }
}