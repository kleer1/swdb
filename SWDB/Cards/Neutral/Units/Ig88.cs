using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;

namespace SWDB.Cards.Neutral.Units
{
    public class Ig88 : NeutralGalaxyUnit, IBountyHunter
    {
        public Ig88(int id, SWDBGame game) :
            base(id, 5, 5, 0, 0, "IG-88", new List<Trait>{ Trait.bountyHunter, Trait.droid }, true, game) {}

        public void ReceiveBounty() 
        {
            if (Owner != null)
            {
                AddExilePendingAction(Owner, 1);
            }
        }
    }
}