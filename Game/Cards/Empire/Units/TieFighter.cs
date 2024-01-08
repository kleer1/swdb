using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;

namespace SWDB.Game.Cards.Empire.Units
{
    public class TieFighter : EmpireGalaxyUnit, IHasAbility
    {
         public TieFighter(int id, SWDBGame game) :
            base(id, 1, 2, 0, 0,"Tie Fighter", new List<Trait>{ Trait.fighter }, false, game) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && (Owner?.ShipsInPlay.Any() ?? false);
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Owner?.DrawCards(1);
        }

        public override int GetTargetValue() 
        {
            return 1;
        }

        public override void ApplyReward() 
        {
            Game.Rebel.AddResources(1);
        }
    }
}