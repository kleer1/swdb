using SWDB.Cards.Common.Models;
using SWDB.Common;
using SWDB.Game;

namespace SWDB.Cards.Rebellion.Units.Starter
{
    public class AllianceShuttle : Unit
    {
        public AllianceShuttle(int id, SWDBGame game) :
            base(id, 0, 0, 1, 0, Faction.rebellion, "Alliance Shuttle", new List<Trait>{ Trait.transport },
                false, game.Rebel, CardLocation.RebelDeck, (IList<Card>) game.Rebel.Deck, game) {}
    }
}