using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Rebellion.Units.Starter
{
    public class AllianceShuttle : Unit
    {
        public AllianceShuttle(int id, SWDBGame game) :
            base(id, 0, 0, 1, 0, Faction.rebellion, "Alliance Shuttle", new List<Trait>{ Trait.transport },
                false, game.Rebel, CardLocation.RebelDeck, game.Rebel.Deck, game) {}
    }
}