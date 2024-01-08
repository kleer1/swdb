using SWDB.Cards.Common;
using SWDB.Cards.Common.Models;
using SWDB.Common;
using SWDB.Game;

namespace SWDB.Cards.Rebellion.Units.Starter
{
    public class TempleGuardian : TempleInquisitor
    {
        public TempleGuardian(int id, SWDBGame game) :
            base(id, Faction.rebellion, "Temple Guardian", Array.Empty<Trait>(), game.Rebel,
                CardLocation.RebelDeck, (IList<Card>) game.Rebel.Deck,  game) {}
    }
}