using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Cards.Empire.Units
{
    public class MoffJerjerrod : EmpireGalaxyUnit, IHasAbility
    {
        public MoffJerjerrod(int id, SWDBGame game) :
            base(id, 4, 2, 2, 0, "Moff Jerjerrod", new List<Trait>{ Trait.officer }, true, game){}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && Game.GalaxyDeck.Any();
        }

        public override void ApplyAbility() 
        {
            if (Owner == null) 
            {
                throw new ArgumentException("Can not apply MoffJerjerrod ability. Card has no owner");
            }
            base.ApplyAbility();
            Game.LookAtTopCardOfDeck(Owner.Faction);
            if (Owner.IsForceWithPlayer()) {
                Game.PendingActions.Add(PendingAction.Of(Action.SwapTopCardOfDeck));
            }
        }

        public override int GetTargetValue() 
        {
            return 4;
        }

        public override void ApplyReward() 
        {
            Game.Rebel.AddForce(3);
        }
    }
}