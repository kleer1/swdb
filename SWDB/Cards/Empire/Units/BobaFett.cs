using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;

namespace SWDB.Cards.Empire.Units
{
    public class BobaFett : EmpireGalaxyUnit, IBountyHunter
    {
        public BobaFett(int id, SWDBGame game) :
            base(id, 5, 5, 0, 0, "Boba Fett", new List<Trait>{ Trait.bountyHunter }, true, game) {}
        
        public override int GetTargetValue() 
        {
            return 5;
        }

        public override void ApplyReward() 
        {
            Game.Rebel.AddResources(3);
            Game.Rebel.AddForce(2);
        }

        public void ReceiveBounty() 
        {
            Owner?.DrawCards(1);
        }
    }
}