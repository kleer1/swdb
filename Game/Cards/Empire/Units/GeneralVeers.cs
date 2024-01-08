using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;

namespace SWDB.Game.Cards.Empire.Units
{
    public class GeneralVeers : EmpireGalaxyUnit, IHasAbility
    {
        public GeneralVeers(int id, SWDBGame game) :
            base(id, 4, 4, 0, 0, "General Veers", new List<Trait>{ Trait.officer }, true, game) {}
        
        public override int GetTargetValue() 
        {
            return 4;
        }

        public override void ApplyReward() 
        {
            Game.Rebel.AddForce(3);
        }

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && 
                (Owner?.UnitsInPlay.Where(c => c.Traits.Contains(Trait.trooper) || c.Traits.Contains(Trait.vehicle)).Any() ?? false);
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Owner?.DrawCards(1);
        }
    }
}