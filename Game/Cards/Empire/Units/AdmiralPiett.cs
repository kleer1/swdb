using SWDB.Game.Cards.Common.Models;

namespace SWDB.Game.Cards.Empire.Units
{
    public class AdmiralPiett : EmpireGalaxyUnit
    {
        public AdmiralPiett(int id, SWDBGame game) :
            base(id, 2, 0, 2, 0, "Admiral Piett", new List<Trait>{ Trait.officer }, true, game) {}
        
        public override void MoveToInPlay() 
        {
            base.MoveToInPlay();
            Game.StaticEffects.Add(StaticEffect.AdmiralPiettBonus);
        }

        public override void MoveToDiscard() 
        {
            base.MoveToDiscard();
            Game.StaticEffects.Remove(StaticEffect.AdmiralPiettBonus);
        }

        public override int GetTargetValue() 
        {
            return 2;
        }

        public override void ApplyReward() 
        {
            Game.Rebel.AddForce(1);
        }
    }
}