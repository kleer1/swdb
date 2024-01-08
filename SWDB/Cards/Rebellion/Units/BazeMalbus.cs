using SWDB.Cards.Common.Models;
using SWDB.Game;

namespace SWDB.Cards.Rebellion.Units
{
    public class BazeMalbus : RebelGalaxyUnit
    {
        public BazeMalbus(int id, SWDBGame game) :
            base(id, 2, 2, 0, 0, "Baze Malbus", new List<Trait>{ Trait.trooper }, true, game) {}

        public override int GetTargetValue() 
        {
            return 2;
        }

        public override void ApplyReward() 
        {
            Game.Empire.AddForce(1);
        }

        public override int Attack
        {
            get
            {
                return base.Attack + Game.Rebel.DestroyedBases.Count;
            }
        }
    }
}