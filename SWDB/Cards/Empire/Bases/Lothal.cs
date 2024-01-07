using SWDB.Cards.Common.Models;
using SWDB.Common;
using SWDB.Game;

namespace SWDB.Cards.Empire.Bases
{
    public class Lothal : Base
    {
        public Lothal(int id, SWDBGame game) :
            base(id, Faction.empire, "Lothal", CardLocation.EmpireCurrentBase, null, game, game.Empire, 8)
        {
            game.Empire.CurrentBase = this;
        }
        
    }
}