using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Game.Cards.Neutral.Units
{
    public class Lobot : NeutralGalaxyUnit, IHasChooseStats
    {
        private Stats? choice;
        public Lobot(int id, SWDBGame game) :
            base(id, 3, 0, 0, 0, "Lobot", new List<Trait>{ Trait.officer }, true, game)
        {
            choice = null;
        }

        public override int Attack 
        {
            get
            {
                return base.Attack + (choice == Stats.Attack ? 2 : 0);
            }
        }

        public override int Resources 
        {
            get
            {
                return base.Resources + (choice == Stats.Resources ? 2 : 0);
            }
            
        }

        public override int Force 
        {
            get
            {
                return base.Force + (choice == Stats.Force ? 2 : 0);
            }
        }
        public override void MoveToDiscard() 
        {
            base.MoveToDiscard();
            choice = null;
        }

        public void ApplyChoice(Stats stat) 
        {
            choice = stat;
            if (choice == Stats.Resources) 
            {
                Owner?.AddResources(2);
            } else if (choice == Stats.Force) 
            {
                Owner?.AddForce(2);
            }
        }

        public IList<PendingAction> GetActions() 
        {
            return new List<PendingAction>{ PendingAction.Of(Action.ChooseStatBoost) };
        }
    }
}