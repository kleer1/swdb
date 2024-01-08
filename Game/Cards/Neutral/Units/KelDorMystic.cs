using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;

namespace SWDB.Game.Cards.Neutral.Units
{
    public class KelDorMystic : NeutralGalaxyUnit, IHasAbility
    {
        public KelDorMystic(int id, SWDBGame game) :
            base(id, 2, 0, 0, 2, "Kel Dor Mystic", Array.Empty<Trait>(), false, game) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && ((Owner?.Hand.Any() ?? false) || (Owner?.Discard.Any() ?? false));
        }

        public override void ApplyAbility() 
        {
            if (Owner == null) 
            {
                throw new ArgumentException("KelDorMystic cannot use exile ability. There is no owner.");
            }
            AddExilePendingAction(Owner, 1);
            MoveToExile();
        }
    }
}