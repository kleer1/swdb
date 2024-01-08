using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;
using SWDB.Common;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Cards.Rebellion.Units
{
    public class PrincessLeia : RebelGalaxyUnit, IHasAbility
    {
        public PrincessLeia(int id, SWDBGame game) :
            base(id, 6, 2, 2, 2, "Princess Leia", new List<Trait>{ Trait.officer }, true, game) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && Game.GalaxyRow.Where(c => c.Faction == Faction.rebellion).Any();
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            if (Owner?.IsForceWithPlayer() ?? false) 
            {
                Game.StaticEffects.Add(StaticEffect.BuyNextToTopOfDeck);
            }
            Game.StaticEffects.Add(StaticEffect.NextFactionPurchaseIsFree);
            Game.PendingActions.Add(PendingAction.Of(Action.PurchaseCard));
        }

        public override int GetTargetValue() 
        {
            return 6;
        }

        public override void ApplyReward() 
        {
            Game.Empire.AddResources(3);
            Game.Empire.AddForce(3);
        }
    }
}