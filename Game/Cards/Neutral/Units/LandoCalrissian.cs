using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Game.Cards.Neutral.Units
{
    public class LandoCalrissian : NeutralGalaxyUnit, IHasAbility
    {
        public LandoCalrissian(int id, SWDBGame game) :
            base(id, 6, 3, 3, 0, "Lando Calrissian", new List<Trait>{ Trait.scoundrel }, true, game) {}

        public override bool AbilityActive() 
        {
            bool active = base.AbilityActive();
            bool hasDrawAbleCard = Owner?.Deck.Count + Owner?.Discard.Count > 0;
            bool oppCanDiscard = (Owner?.IsForceWithPlayer() ?? false) && (Owner?.Opponent?.Hand.Any() ?? false);
            return active && (hasDrawAbleCard || oppCanDiscard);
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Owner?.DrawCards(1);
            if ((Owner?.IsForceWithPlayer() ?? false) && (Owner?.Opponent?.Hand.Any() ?? false)) 
            {
                Game.PendingActions.Add(PendingAction.Of(Action.DiscardFromHand, true));
            }
        }
    }
}