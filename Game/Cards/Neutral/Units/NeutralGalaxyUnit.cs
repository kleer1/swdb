using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Neutral.Units
{
    public class NeutralGalaxyUnit : Unit
    {
        protected NeutralGalaxyUnit(int id, int cost, int attack, int resources, int force,
                string title, IReadOnlyCollection<Trait> traits, bool isUnique, SWDBGame game) :
            base(id, cost, attack, resources, force, Faction.neutral, title, traits, isUnique, null,
                CardLocation.GalaxyDeck, game.GalaxyDeck, game) {}
    }
}