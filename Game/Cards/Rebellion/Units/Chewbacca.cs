using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;

namespace SWDB.Game.Cards.Rebellion.Units
{
    public class Chewbacca : RebelGalaxyUnit, IHasAbility
    {
        public Chewbacca(int id, SWDBGame game) :
            base(id, 4, 5, 0, 0, "Chewbacca", new List<Trait>{ Trait.scoundrel }, true, game) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && 
                (Owner?.UnitsInPlay.Where(c => c is not Chewbacca && c.IsUnique).Any() ?? false);
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Owner?.DrawCards(1);
        }

        public override int GetTargetValue() 
        {
            return 4;
        }

        public override void ApplyReward() 
        {
            Game.Empire.AddForce(3);
        }
    }
}