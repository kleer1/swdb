using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Game.Cards.Empire.Units
{
    public class AtAt : EmpireGalaxyUnit, IHasReturnToHandAbility
    {
        public AtAt(int id, SWDBGame game) :
            base(id, 6, 6, 0, 0, "AT-AT", new List<Trait> { Trait.vehicle }, false, game) {}

        public override bool AbilityActive()
        {
            return base.AbilityActive() && (Owner?.Discard.BaseList.Where(c => c.Traits.Contains(Trait.trooper)).Any() ?? false);
        }

        public override void ApplyAbility()
        {
            base.ApplyAbility();
            Game.PendingActions.Add(PendingAction.Of(Action.ReturnCardToHand));
        }

        public override int GetTargetValue()
        {
            return 6;
        }

        public override void ApplyReward()
        {
            Game.PendingActions.Add(PendingAction.Of(Action.PurchaseCard));
            Game.StaticEffects.Add(StaticEffect.NextFactionPurchaseIsFree);
        }

        public bool IsValidTarget(PlayableCard card)
        {
            return card.Traits.Contains(Trait.trooper);
        }
    }
}