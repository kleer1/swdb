using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Rebellion.Units.Starter
{
    public class RebelTrooper : Unit
    {
        public RebelTrooper(int id, SWDBGame game) :
            base(id, 0, 2, 0, 0, Faction.rebellion, "Rebel Trooper", new List<Trait>{ Trait.trooper },
                false, game.Rebel, CardLocation.RebelDeck, (IList<Card>) game.Rebel.Deck, game) {}
    }
}