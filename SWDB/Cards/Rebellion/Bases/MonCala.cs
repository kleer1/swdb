using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Common;
using SWDB.Game;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Cards.Rebellion.Bases
{
    public class MonCala : Base, IHasOnReveal
    {
        public MonCala(int id, SWDBGame game) :
            base(id, Faction.rebellion, "Mon Cala", CardLocation.RebelAvailableBases, (IList<Card>) game.Rebel.AvailableBases,
                game, game.Rebel, 10) {}

        public void ApplyOnReveal() 
        {
            Game.PendingActions.Add(PendingAction.Of(Action.PurchaseCard));
            Game.StaticEffects.Add(StaticEffect.NextFactionOrNeutralPurchaseIsFree);
            Game.StaticEffects.Add(StaticEffect.BuyNextToHand);
        }
    }
}