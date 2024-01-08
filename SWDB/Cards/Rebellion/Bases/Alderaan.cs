using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Common;
using SWDB.Game;

namespace SWDB.Cards.Rebellion.Bases
{
    public class Alderaan : Base, IHasOnReveal
    {
        public Alderaan(int id, SWDBGame game) :
            base(id, Faction.rebellion, "Alderaan", CardLocation.RebelAvailableBases, (IList<Card>) game.Rebel.AvailableBases,
                game, game.Rebel, 14) {}

        public void ApplyOnReveal() 
        {
            Owner?.AddForce(4);
        }
    }
}