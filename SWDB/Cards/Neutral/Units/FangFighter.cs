using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;

namespace SWDB.Cards.Neutral.Units
{
    public class FangFighter : NeutralGalaxyUnit, IHasPurchaseAction
    {
        public FangFighter(int id, SWDBGame game) :
            base(id, 3, 3, 0, 0, "Fang Fighter", new List<Trait>{ Trait.fighter }, false, game) {}

        public void ApplyPurchaseAction() 
        {
            MoveToHand();
            if (Owner?.IsForceWithPlayer() ?? false) 
            {
                Owner?.DrawCards(1);
            }
        }
    }
}