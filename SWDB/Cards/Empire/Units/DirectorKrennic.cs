using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Cards.Empire.Bases;
using SWDB.Game;

namespace SWDB.Cards.Empire.Units
{
    public class DirectorKrennic : EmpireGalaxyUnit, IHasAbility
    {
        public DirectorKrennic(int id, SWDBGame game) :
            base(id, 5, 3, 2, 0, "Director Krennic", new List<Trait>{ Trait.officer }, true, game) {}

        public override void ApplyAbility() {
            base.ApplyAbility();
            int amountToDraw = 1;
            if (Owner?.CurrentBase?.GetType() == typeof(DeathStar)) 
            {
                amountToDraw = 2;
            }
            Owner?.DrawCards(amountToDraw);
        }

        public override int GetTargetValue() {
            return 5;
        }

        public override void ApplyReward() {
            Game.Rebel.AddResources(3);
            Game.Rebel.AddForce(2);
        }
    }
}