using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Game.Cards.Neutral.Units
{
    public class JabbasSailBarge : NeutralGalaxyUnit, IHasReturnToHandAbility
    {
        public JabbasSailBarge(int id, SWDBGame game) :
            base(id, 7, 4, 3, 0, "Jabbas Sail Barge", new List<Trait>{ Trait.vehicle }, true, game) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && (Owner?.Discard.Where(pc => pc.Traits.Contains(Trait.bountyHunter)).Any() ?? false);
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Game.PendingActions.Add(PendingAction.Of(Action.ReturnCardToHand));
        }

        public bool IsValidTarget(PlayableCard card) 
        {
            return card.Traits.Contains(Trait.bountyHunter);
        }
    }
}