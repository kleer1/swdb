using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;

namespace SWDB.Cards.Neutral.Units
{
    public class Hwk290 : NeutralGalaxyUnit, IHasAbility
    {
        public Hwk290(int id, SWDBGame game) :
            base(id, 4, 0, 4, 0, "HWK-290", new List<Trait>{ Trait.transport }, false, game) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && Owner?.CurrentBase?.CurrentDamage > 0;
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Owner?.CurrentBase?.AddDamage(-4);
            MoveToExile();
        }
    }
}