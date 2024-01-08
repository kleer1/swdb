using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;

namespace SWDB.Cards.Neutral.Units
{
    public class Bossk : NeutralGalaxyUnit, IBountyHunter
    {
        public Bossk(int id, SWDBGame game) :
            base(id, 3, 3, 0, 0, "Bossk", new List<Trait>{ Trait.bountyHunter }, true, game) {}

        public void ReceiveBounty() 
        {
            Owner?.AddForce(1);
        }
    }
}