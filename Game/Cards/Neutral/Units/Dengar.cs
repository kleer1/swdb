using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;

namespace SWDB.Game.Cards.Neutral.Units
{
    public class Dengar : NeutralGalaxyUnit, IBountyHunter
    {
         public Dengar(int id, SWDBGame game) :
            base(id, 4, 4, 0, 0, "Dengar", new List<Trait>{ Trait.bountyHunter }, true, game) {}

        public void ReceiveBounty() 
        {
            Owner?.AddResources(2);
        }
    }
}