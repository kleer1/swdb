using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;

namespace SWDB.Cards.Rebellion.Units
{
    public class YWing : RebelGalaxyUnit, IHasAbility
    {
        public YWing(int id, SWDBGame game) :
            base(id, 1, 2, 0, 0, "Y Wing", new List<Trait>{ Trait.fighter }, false, game) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() &&
                (Game.Empire.CurrentBase != null || Game.Empire.ShipsInPlay.Any());
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            if (Game.Empire.CurrentBase != null) 
            {
                Game.Empire.CurrentBase.AddDamage(2);
            } else 
            {
                Game.AssignDamageToBase(2, Game.Empire);
            }
            MoveToExile();
        }

        public override int GetTargetValue() 
        {
            return 1;
        }

        public override void ApplyReward() 
        {
            Game.Empire.AddResources(1);
        }
    }
}