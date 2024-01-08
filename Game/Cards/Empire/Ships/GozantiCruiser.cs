using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Game.Cards.Empire.Ships
{
    public class GozantiCruiser : EmpireGalaxyShip, IHasAbility
    {
        public GozantiCruiser(int id, SWDBGame game) :
            base(id, 3, 0, 2, 0, "Gozanti Cruiser", game, 3) {}
        
        public override bool AbilityActive() 
        {
            return base.AbilityActive() && (Owner?.Hand?.Any() ?? false);
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Game.PendingActions.Add(PendingAction.Of(Action.DiscardFromHand, () => Owner?.DrawCards(1)));
        }
    }
}