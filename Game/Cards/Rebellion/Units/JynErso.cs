using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Game.Cards.Rebellion.Units
{
    public class JynErso : RebelGalaxyUnit, IHasAbility
    {
        public JynErso(int id, SWDBGame game) :
            base(id, 4, 4, 0, 0, "Jyn Erso", new List<Trait>{ Trait.scoundrel }, true, game) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && (Owner?.Opponent?.Hand.Any() ?? false);
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Game.RevealOpponentsHand();
            if (Owner?.IsForceWithPlayer() ?? false) 
            {
                Game.PendingActions.Add(PendingAction.Of(Action.JynErsoTopDeck));
            }
        }

        public override int GetTargetValue() 
        {
            return 4;
        }

        public override void ApplyReward() 
        {
            Game.Empire.AddForce(3);
        }
    }
}