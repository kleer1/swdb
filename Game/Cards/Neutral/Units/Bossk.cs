using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;

namespace SWDB.Game.Cards.Neutral.Units
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