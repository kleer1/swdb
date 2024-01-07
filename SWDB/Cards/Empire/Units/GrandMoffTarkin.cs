using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;
using SWDB.Common;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Cards.Empire.Units
{
    public class GrandMoffTarkin : EmpireGalaxyUnit, IHasAbility
    {
        public GrandMoffTarkin(int id, SWDBGame game) :
            base(id, 6, 2, 2, 2, "Grand Moff Tarkin", new List<Trait>{ Trait.officer }, true, game) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && Game.GalaxyRow.Where(pc => pc.Faction == Faction.empire).Any();
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Game.PendingActions.Add(PendingAction.Of(Action.PurchaseCard));
            Game.StaticEffects.Add(StaticEffect.NextFactionPurchaseIsFree);
            Game.StaticEffects.Add(StaticEffect.ExileNextFactionPurchase);
            Game.StaticEffects.Add(StaticEffect.BuyNextToHand);
        }

        public override int GetTargetValue() 
        {
            return 6;
        }

        public override void ApplyReward() 
        {
            Game.Rebel.AddResources(3);
            Game.Rebel.AddForce(3);
        }
        
    }
}