using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Rebellion.Ships
{
    public class RebelGalaxyShip : CapitalShip
    {
        protected RebelGalaxyShip(int id, int cost, int attack, int resources, int force, string title, SWDBGame game, int hitPoints) :
            base(id, cost, attack, resources, force, Faction.rebellion, title, new List<Trait>(), false, null,
                CardLocation.GalaxyDeck, game.GalaxyDeck, game, hitPoints) {}
    }
}