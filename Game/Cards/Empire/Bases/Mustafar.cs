using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Empire.Bases
{
    public class Mustafar : Base, IHasOnReveal
    {
        public Mustafar(int id, SWDBGame game) :
            base(id, Faction.empire, "Mustafar", CardLocation.EmpireAvailableBases, game.Empire.AvailableBases,
                game, game.Empire, 14) {}
        
        public void ApplyOnReveal() 
        {
            Game.Empire.AddForce(4);
        }
    }
}