using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;
using SWDB.Game.Utils;

namespace SWDB.Game.Cards.Empire.Units.Starter
{
    public class Stormtrooper : Unit
    {
        public Stormtrooper(int id, SWDBGame game) :
            base(id, 0, 2, 0, 0, Faction.empire, "Stormtrooper", new List<Trait>{ Trait.trooper },
                false, game.Empire, CardLocation.EmpireDeck, game.Empire.Deck, game) {}
    }
}