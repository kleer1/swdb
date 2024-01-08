using SWDB.Game.Cards.Common;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Rebellion.Units.Starter
{
    public class TempleGuardian : TempleInquisitor
    {
        public TempleGuardian(int id, SWDBGame game) :
            base(id, Faction.rebellion, "Temple Guardian", Array.Empty<Trait>(), game.Rebel,
                CardLocation.RebelDeck, (IList<Card>) game.Rebel.Deck,  game) {}
    }
}