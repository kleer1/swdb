using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Game.Cards.Empire.Units
{
    public class LandingCraft : EmpireGalaxyUnit, ITargetable, IHasChooseResourceOrRepair
    {
        public LandingCraft(int id, SWDBGame game) :
            base(id, 4, 0, 0, 0, "Landing Craft", new List<Trait>{ Trait.transport }, false, game) {}
        
        public void ApplyChoice(ResourceOrRepair choice) 
        {
            if (choice == ResourceOrRepair.Repair) 
            {
                Owner?.CurrentBase?.AddDamage(-4);
            } else if (choice == ResourceOrRepair.Resources) 
            {
                Owner?.AddResources(4);
            }
        }

        public override int GetTargetValue() 
        {
            return 4;
        }

        public override void ApplyReward() 
        {
            Game.Rebel.AddResources(4);
        }
        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            if (Owner?.CurrentBase?.CurrentDamage >  0) 
            {
                Game.PendingActions.Add(PendingAction.Of(Action.ChooseResourceOrRepair));
            } else 
            {
                Owner?.AddResources(4);
            }
        }
    }
}