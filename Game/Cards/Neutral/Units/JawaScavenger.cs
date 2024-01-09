using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Game.Cards.Neutral.Units
{
    public class JawaScavenger : NeutralGalaxyUnit, IHasAbility
    {
        public JawaScavenger(int id, SWDBGame game) :
            base(id, 1, 0, 2, 0, "Jawa Scavenger", Array.Empty<Trait>(), false, game) {}

        public override bool AbilityActive() 
        {
            if (!base.AbilityActive()) 
            {
                return false;
            }
            if (!Game.GalaxyDiscard.Any()) return false;

            var tempList = Game.GalaxyDiscard.BaseList.Where(pc => pc.Faction == Owner?.Faction || pc.Faction == Faction.neutral).ToList();
            if (!tempList.Any()) return false;

            int minValue = tempList.Min(pc => pc.Cost);
            return minValue <= Owner?.Resources;
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            MoveToExile();
            Game.PendingActions.Add(PendingAction.Of(Action.PurchaseCard));
            Game.StaticEffects.Add(StaticEffect.PurchaseFromDiscard);
        }
    }
}