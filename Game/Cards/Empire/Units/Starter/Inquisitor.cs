using SWDB.Game.Cards.Common;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Empire.Units.Starter
{
    public class Inquisitor : TempleInquisitor
    {
        public Inquisitor(int id, SWDBGame game) :
            base(id,  Faction.empire, "Inquisitor", Array.Empty<Trait>(), game.Empire, 
                CardLocation.EmpireDeck, (IList<Card>) game.Empire.Deck, game) {}
    }
}