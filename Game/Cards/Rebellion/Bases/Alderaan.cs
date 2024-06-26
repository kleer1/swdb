using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Rebellion.Bases
{
    public class Alderaan : Base, IHasOnReveal
    {
        public Alderaan(int id, SWDBGame game) :
            base(id, Faction.rebellion, "Alderaan", CardLocation.RebelAvailableBases, game.Rebel.AvailableBases,
                game, game.Rebel, 14) {}

        public void ApplyOnReveal() 
        {
            Owner?.AddForce(4);
        }
    }
}