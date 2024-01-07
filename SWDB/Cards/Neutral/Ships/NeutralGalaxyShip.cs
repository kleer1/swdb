using SWDB.Cards.Common.Models;
using SWDB.Common;
using SWDB.Game;

namespace SWDB.Cards.Neutral.Ships
{
    public class NeutralGalaxyShip : CapitalShip
    {
        protected NeutralGalaxyShip(int id, int cost, int attack, int resources, string title, SWDBGame game, int hitPoints) :
            base(id, cost, attack, resources, 0, Faction.neutral, title, new List<Trait>(), false, null,
                CardLocation.GalaxyDeck, (IList<Card>) game.GalaxyDeck, game, hitPoints) {}
    }
}