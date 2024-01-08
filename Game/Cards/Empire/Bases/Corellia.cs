using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Game.Cards.Empire.Bases
{
    public class Corellia : Base, IHasOnReveal
    {
        public Corellia(int id, SWDBGame game) :
        base(id, Faction.empire, "Corellia", CardLocation.EmpireAvailableBases, (IList<Card>) game.Empire.AvailableBases,
                game, game.Empire,10) {}
        
        public void ApplyOnReveal() 
        {
            Game.PendingActions.Add(PendingAction.Of(Action.PurchaseCard));
            Game.StaticEffects.Add(StaticEffect.NextFactionOrNeutralPurchaseIsFree);
            Game.StaticEffects.Add(StaticEffect.BuyNextToHand);
        }
    }
}