using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Game.Cards.Neutral.Units
{
    public class JabbaTheHutt : NeutralGalaxyUnit, IHasAbility
    {
        public JabbaTheHutt(int id, SWDBGame game) :
            base(id, 8, 2, 2, 2, "Jabba The Hutt", new List<Trait>{ Trait.scoundrel }, true, game) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && (Owner?.Hand.Any() ?? false) && Owner?.Discard.Count + Owner?.Deck.Count > 0;
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Game.PendingActions.Add(PendingAction.Of(Action.JabbaExile, () => 
            {
                if (Owner?.IsForceWithPlayer() ?? false) 
                {
                    Owner?.DrawCards(2);
                } else 
                {
                    Owner?.DrawCards(1);
                }
            }));
        }
    }
}