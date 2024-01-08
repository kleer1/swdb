using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;

namespace SWDB.Game.Cards.Rebellion.Units
{
    public class UWing : RebelGalaxyUnit, IHasAbility
    {
        public UWing(int id, SWDBGame game) :
            base(id, 4, 0, 3, 0, "U-Wing", new List<Trait>{ Trait.fighter }, false, game) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && Owner?.IsForceWithPlayer() == true &&
                Owner?.CurrentBase?.CurrentDamage > 0;
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Owner?.CurrentBase?.AddDamage(-3);
        }

        public override int GetTargetValue() 
        {
            return 4;
        }

        public override void ApplyReward() 
        {
            Game.Empire.AddResources(4);
        }
    }
}