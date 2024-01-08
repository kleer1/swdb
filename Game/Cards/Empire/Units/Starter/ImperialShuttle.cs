using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Empire.Units.Starter
{
    public class ImperialShuttle : Unit
    {
        public ImperialShuttle(int id, SWDBGame game) :
            base(id, 0, 0, 1, 0, Faction.empire, "Imperial Shuttle", new List<Trait>{ Trait.transport },
                false, game.Empire, CardLocation.EmpireDeck, (IList<Card>) game.Empire.Deck, game) {}
    }
}