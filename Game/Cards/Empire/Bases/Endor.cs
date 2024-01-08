using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Empire.Bases
{
    public class Endor : Base, IHasAtStartOfTurn, IHasOnReveal
    {
        public Endor(int id, SWDBGame game) :
            base(id, Faction.empire, "Endor", CardLocation.EmpireAvailableBases, (IList<Card>) game.Empire.AvailableBases,
                game, game.Empire, 16) {}

        public void ApplyAtStartOfTurn() 
        {
            Game.StaticEffects.Add(StaticEffect.EndorBonus);
        }

        public void ApplyOnReveal() 
        {
            Game.StaticEffects.Add(StaticEffect.EndorBonus);
        }
    }
}