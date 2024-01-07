using SWDB.Cards.Common.Models;
using SWDB.Common;
using SWDB.Game;

namespace SWDB.Cards.Empire.Units.Starter
{
    public class Stormtrooper : Unit
    {
        public Stormtrooper(int id, SWDBGame game) :
            base(id, 0, 2, 0, 0, Faction.empire, "Stormtrooper", new List<Trait>{ Trait.trooper },
                false, game.Empire, CardLocation.EmpireDeck, (List<Card>) game.Empire.Deck, game) {}
    }
}