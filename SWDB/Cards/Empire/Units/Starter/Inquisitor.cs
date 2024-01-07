using SWDB.Cards.Common;
using SWDB.Cards.Common.Models;
using SWDB.Common;
using SWDB.Game;

namespace SWDB.Cards.Empire.Units.Starter
{
    public class Inquisitor : TempleInquisitor
    {
        public Inquisitor(int id, SWDBGame game) :
            base(id,  Faction.empire, "Inquisitor", Array.Empty<Trait>(), game.Empire, 
                CardLocation.EmpireDeck, (IList<Card>) game.Empire.Deck, game) {}
    }
}