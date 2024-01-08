using SWDB.Cards.Common.Models;
using SWDB.Common;
using SWDB.Game;

namespace SWDB.Cards.Neutral.Units
{
    public class RodianGunslinger : NeutralGalaxyUnit
    {
        public RodianGunslinger(int id, SWDBGame game) :
            base(id, 2, 2, 0, 0, "Rodian Gunslinger", new List<Trait>{ Trait.bountyHunter }, false, game) {}
            
        public override int Attack 
        {
            get
            {
                int atk = base.Attack;
                if (Game?.AttackTarget?.Location == CardLocation.GalaxyRow) {
                    atk += 2;
                }
                return atk;
            }
        }
    }
}