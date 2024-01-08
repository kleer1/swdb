using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;

namespace SWDB.Game.Cards.Neutral.Units
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