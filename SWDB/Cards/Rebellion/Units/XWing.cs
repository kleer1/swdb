using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;

namespace SWDB.Cards.Rebellion.Units
{
    public class XWing : RebelGalaxyUnit, IHasAbility
    {
        public XWing(int id, SWDBGame game) :
            base(id, 3, 3, 0, 0, "X-Wing", new List<Trait>{ Trait.fighter }, false, game) {}

        public override bool AbilityActive() 
        {
            return base.AbilityActive() && Owner?.IsForceWithPlayer() == true;
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Owner?.DrawCards(1);
        }

        public override int GetTargetValue() 
        {
            return 3;
        }

        public override void ApplyReward() 
        {
            Game.Empire.AddResources(3);
        }
    }
}