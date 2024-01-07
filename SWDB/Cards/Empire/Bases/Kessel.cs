using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Common;
using SWDB.Game;

namespace SWDB.Cards.Empire.Bases
{
    public class Kessel : Base, IHasOnReveal
    {
        public Kessel(int id, SWDBGame game) :
            base(id, Faction.empire, "Kessel", CardLocation.EmpireAvailableBases, (IList<Card>) game.Empire.AvailableBases,
                game, game.Empire, 12) {}
        
        public void ApplyOnReveal() {
            if (Owner == null)
            {
                throw new ArgumentException("No owner to appy reveal for Kessel");
            }
            AddExilePendingAction(Owner, 3);
        }
    }
}