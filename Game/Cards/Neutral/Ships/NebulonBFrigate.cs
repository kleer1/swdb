using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Game.Cards.Neutral.Ships
{
    public class NebulonBFrigate : NeutralGalaxyShip, IHasAbility, IHasChooseResourceOrRepair
    {
        public NebulonBFrigate(int id, SWDBGame game) :
            base(id, 5, 0, 0, "Nebulon-B Frigate", game, 5) {}

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            if (Owner?.CurrentBase?.CurrentDamage > 0) 
            {
                Game.PendingActions.Add(PendingAction.Of(Action.ChooseResourceOrRepair));
            } else 
            {
                Owner?.AddResources(3);
            }
        }

        public void ApplyChoice(ResourceOrRepair choice) 
        {
            if (choice == ResourceOrRepair.Repair) {
                Owner?.CurrentBase?.AddDamage(-3);
            } else if (choice == ResourceOrRepair.Resources) 
            {
                Owner?.AddResources(3);
            }
        }
    }
}