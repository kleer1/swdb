using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Empire.Ships
{
    public class EmpireGalaxyShip : CapitalShip
    {
        protected EmpireGalaxyShip(int id, int cost, int attack, int resources, int force, string title, SWDBGame game, int hitPoints) :
            base(id, cost, attack, resources, force, Faction.empire, title, new List<Trait>(), false, null,
                CardLocation.GalaxyDeck, game.GalaxyDeck, game, hitPoints) {}
    }
}