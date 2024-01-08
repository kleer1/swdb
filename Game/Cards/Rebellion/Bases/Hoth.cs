using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Rebellion.Bases
{
    public class Hoth : Base, IHasAtStartOfTurn
    {
        private int damageTakenThisTurn;
        public Hoth(int id, SWDBGame game) :
            base(id, Faction.rebellion, "Hoth", CardLocation.RebelAvailableBases, (IList<Card>) game.Rebel.AvailableBases, 
                game, game.Rebel, 14)
        {
            damageTakenThisTurn = 0;
        }

        public void ApplyAtStartOfTurn() 
        {
            damageTakenThisTurn = 0;
        }

        public override void AddDamage(int damage) 
        {
            int dam = damage;
            if (damageTakenThisTurn < 2) 
            {
                dam -= 2 - damageTakenThisTurn;
                if (dam < 0) 
                {
                    dam = 0;
                }
                damageTakenThisTurn += damage;
            }
            base.AddDamage(dam);
        }
    }
}