using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Empire.Bases
{
    public class Kafrene : Base, IHasAtStartOfTurn, IHasOnReveal
    {
        public Kafrene(int id, SWDBGame game) :
            base(id, Faction.empire, "Kafrene", CardLocation.EmpireAvailableBases, game.Empire.AvailableBases,
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