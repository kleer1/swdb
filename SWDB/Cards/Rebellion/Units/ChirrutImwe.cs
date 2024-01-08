using SWDB.Cards.Common.Models;
using SWDB.Game;

namespace SWDB.Cards.Rebellion.Units
{
    public class ChirrutImwe : RebelGalaxyUnit
    {
        public ChirrutImwe(int id, SWDBGame game) :
            base(id, 3, 0, 0, 2, "Chirrut Imwe", new List<Trait>{ Trait.trooper }, true, game) {}

        public override int GetTargetValue() 
        {
            return 3;
        }

        public override void ApplyReward() 
        {
            Game.Empire.AddForce(2);
        }

        public override int Attack 
        {
            get
            {
                return base.Attack + (Owner != null && Owner.IsForceWithPlayer() ? 2 : 0);
            }
        }
    }
}