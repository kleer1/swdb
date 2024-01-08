using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Empire.Bases
{
    public class OrdMantell : Base, IHasOnReveal, IHasAtStartOfTurn
    {
        public OrdMantell(int id, SWDBGame game) :
            base(id, Faction.empire, "Ord Mantell", CardLocation.EmpireAvailableBases, (IList<Card>) game.Empire.AvailableBases,
                game, game.Empire, 14) {}
        
        public void ApplyAtStartOfTurn() 
        {
            Game.StaticEffects.Add(StaticEffect.CanBountyOneNeutral);
        }

        public void ApplyOnReveal() 
        {
            Game.StaticEffects.Add(StaticEffect.CanBountyOneNeutral);
        }
    }
}