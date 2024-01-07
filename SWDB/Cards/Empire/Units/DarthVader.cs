using SWDB.Cards.Common.Models;
using SWDB.Game;

namespace SWDB.Cards.Empire.Units
{
    public class DarthVader : EmpireGalaxyUnit
    {
        public DarthVader(int id, SWDBGame game) :
            base(id, 8, 6, 0, 2, "Darth Vader", new List<Trait>{ Trait.sith }, true, game) {}
        
        public override int GetTargetValue() 
        {
            return 8;
        }

        public override void ApplyReward() 
        {
            Game.Rebel.AddResources(4);
            Game.Rebel.AddForce(4);
        }

        public override int Attack 
        {
            get
            {
                int atk = base.Attack;
                if (Owner != null && Owner.IsForceWithPlayer()) 
                {
                    atk += 4;
                }
                return atk;
            }
            
        }
    }
}