using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Rebellion.Bases
{
    public class Dagobah : Base, IHasOnReveal
    {
        public Dagobah(int id, SWDBGame game) :
            base(id, Faction.rebellion, "Dagobah", CardLocation.RebelAvailableBases, game.Rebel.AvailableBases,
                game, game.Rebel, 12) {}
        public void ApplyOnReveal() 
        {
            if (Owner == null)
            {
                throw new ArgumentException("Dagobah can not use exile ability. It has no owener");
            }
            AddExilePendingAction(Owner, 3);
        }
    }
}