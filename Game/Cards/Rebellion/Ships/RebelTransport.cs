using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Game.Cards.Rebellion.Ships
{
    public class RebelTransport : RebelGalaxyShip, IHasChooseResourceOrRepair
    {
        public RebelTransport(int id, SWDBGame game) :
            base(id, 2, 0, 0, 0, "Rebel Transport", game, 2) {}

        public void ApplyChoice(ResourceOrRepair choice) 
        {
            switch (choice) 
            {
                case ResourceOrRepair.Repair:
                    Owner?.CurrentBase?.AddDamage(-2);
                    break;
                case ResourceOrRepair.Resources:
                    Owner?.AddResources(1);
                    break;
            }
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            if (Owner?.CurrentBase?.CurrentDamage > 0)
            {
                Game.PendingActions.Add(PendingAction.Of(Action.ChooseResourceOrRepair));
            } else 
            {
                Owner?.AddResources(1);
            }
        }
    }
}