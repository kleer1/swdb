using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWDB.Cards.Common.Models;
using SWDB.Common;
using SWDB.Game;

namespace SWDB.Cards.Rebellion.Ships
{
    public class RebelGalaxyShip : CapitalShip
    {
        protected RebelGalaxyShip(int id, int cost, int attack, int resources, int force, string title, SWDBGame game, int hitPoints) :
            base(id, cost, attack, resources, force, Faction.rebellion, title, new List<Trait>(), false, null,
                CardLocation.GalaxyDeck, (IList<Card>) game.GalaxyDeck, game, hitPoints) {}
    }
}