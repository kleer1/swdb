using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Rebellion.Bases
{
    public class Dantooine : Base
    {
        public Dantooine(int id, SWDBGame game) :
            base(id, Faction.rebellion, "Dantooine", CardLocation.RebelCurrentBase, null, game, game.Rebel, 8)
        {
            game.Rebel.CurrentBase = this;
        }
    }
}