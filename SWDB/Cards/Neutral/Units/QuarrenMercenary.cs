using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;

namespace SWDB.Cards.Neutral.Units
{
    public class QuarrenMercenary : NeutralGalaxyUnit, IHasPurchaseAction
    {
        public QuarrenMercenary(int id, SWDBGame game) :
            base(id, 4, 4, 0, 0, "Quarren Mercenary", new List<Trait>{ Trait.trooper }, false, game) {}

        public void ApplyPurchaseAction() 
        {
            if (Owner == null) 
            {
                throw new ArgumentException("QuarrenMercenary cannot use exile on purchase action. Ithas no owner.");
            }
            AddExilePendingAction(Owner, Owner.IsForceWithPlayer() ? 2 : 1);
        }
    }
}