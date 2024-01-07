using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Common;
using SWDB.Game;

namespace SWDB.Cards.Empire.Bases
{
    public class Kafrene : Base, IHasAtStartOfTurn, IHasOnReveal
    {
        public Kafrene(int id, SWDBGame game) :
            base(id, Faction.empire, "Kafrene", CardLocation.EmpireAvailableBases, (IList<Card>) game.Empire.AvailableBases,
                    game, game.Empire, 14) {}
        
        public void ApplyAtStartOfTurn() 
        {
            Game.StaticEffects.Add(StaticEffect.DrawOnFirstNeutralCard);
        }

        public void ApplyOnReveal() 
        {
            Game.StaticEffects.Add(StaticEffect.DrawOnFirstNeutralCard);
        }
    }
}