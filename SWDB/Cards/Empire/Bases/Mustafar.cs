using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Common;
using SWDB.Game;

namespace SWDB.Cards.Empire.Bases
{
    public class Mustafar : Base, IHasOnReveal
    {
        public Mustafar(int id, SWDBGame game) :
            base(id, Faction.empire, "Mustafar", CardLocation.EmpireAvailableBases, (IList<Card>) game.Empire.AvailableBases,
                game, game.Empire, 14) {}
        
        public void ApplyOnReveal() 
        {
            Game.Empire.AddForce(4);
        }
    }
}