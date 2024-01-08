using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Game.Cards.Rebellion.Units
{
    public class MillenniumFalcon : RebelGalaxyUnit, IHasReturnToHandAbility
    {
        public MillenniumFalcon(int id, SWDBGame game) :
            base(id, 7, 5, 2, 0, "Millennium Falcon", new List<Trait>{ Trait.transport }, true, game) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && (Owner?.Discard.Where(c => c.IsUnique).Any() ?? false);
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Game.PendingActions.Add(PendingAction.Of(Action.ReturnCardToHand));
        }

        public override int GetTargetValue() 
        {
            return 7;
        }

        public override void ApplyReward() 
        {
            Game.PendingActions.Add(PendingAction.Of(Action.PurchaseCard));
            Game.StaticEffects.Add(StaticEffect.NextFactionPurchaseIsFree);
        }

        public bool IsValidTarget(PlayableCard card) 
        {
            return card.IsUnique;
        }
    }
}