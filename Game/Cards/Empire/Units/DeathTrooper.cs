using SWDB.Game.Cards.Common.Models;

namespace SWDB.Game.Cards.Empire.Units
{
    public class DeathTrooper : EmpireGalaxyUnit
    {
        public DeathTrooper(int id, SWDBGame game) :
            base(id, 3, 3, 0, 0, "Death Trooper", new List<Trait>{ Trait.trooper }, false, game) {}

        public override int Attack 
        {
            get
            {
                if (Owner != null && Owner.IsForceWithPlayer()) {
                    return base.Attack + 2;
                }
                return base.Attack;
            }
        }

        public override int GetTargetValue() 
        {
            return 3;
        }

        public override void ApplyReward() 
        {
            Game.ForceBalance.LightSideGainForce(2);
        }
    }
}