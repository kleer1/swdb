using SWDB.Cards.Common.Models;
using SWDB.Common;
using SWDB.Game;

namespace SWDB.Cards.Neutral.Units
{
    public class NeutralGalaxyUnit : Unit
    {
        protected NeutralGalaxyUnit(int id, int cost, int attack, int resources, int force,
                String title, List<Trait> traits, bool isUnique, SWDBGame game) :
            base(id, cost, attack, resources, force, Faction.neutral, title, traits, isUnique, null,
                CardLocation.GalaxyDeck, (IList<Card>) game.GalaxyDeck, game) {}
    }
}