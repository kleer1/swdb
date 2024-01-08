using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;

namespace SWDB.Cards.Rebellion.Units
{
    public class HanSolo : RebelGalaxyUnit, IHasAbility
    {
        public HanSolo(int id, SWDBGame game) :
            base(id, 5, 3, 2, 0, "Han Solo", new List<Trait>{ Trait.scoundrel }, true, game) {}


        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            if (Owner?.UnitsInPlay.Where(u => u.GetType() == typeof(MillenniumFalcon)).Any() ?? false) 
            {
                Owner?.DrawCards(2);
            } else 
            {
                Owner?.DrawCards(1);
            }
        }

        public override int GetTargetValue() 
        {
            return 5;
        }

        public override void ApplyReward() 
        {
            Game.Empire.AddResources(3);
            Game.Empire.AddForce(2);
        }
    }
}