using SWDB.Game;

namespace SWDB.Cards.Empire.Ships
{
    public class StarDestroyer : EmpireGalaxyShip
    {
        public StarDestroyer(int id, SWDBGame game) :
            base(id, 7, 4, 0, 0, "StarDestroyer", game, 7) {}
    }
}