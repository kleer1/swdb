using Game.Cards.Common.Models.Interface;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;

namespace SWDB.Game.Cards.Empire.Bases
{
    public class Rodia : Base, IHasOnReveal
    {
        public Rodia(int id, SWDBGame game) :
            base(id, Faction.empire, "Rodia", CardLocation.EmpireAvailableBases, game.Empire.AvailableBases,
                game, game.Empire, 16) {}
        

        public void ApplyOnReveal() 
        {
            int numMatches = 0;
            for (int i = Game.GalaxyRow.Count - 1; i >= 0; i--)
            {
                IPlayableCard card = Game.GalaxyRow.BaseList[i];
                if (card.Faction == Faction.rebellion) 
                {
                    Game.GalaxyRow.RemoveAt(i);
                    numMatches++;
                    card.MoveToGalaxyDiscard();
                }
            }
            Game?.Rebel?.CurrentBase?.AddDamage(numMatches);
            for (int i = 0; i < numMatches; i++) 
            {
                Game?.DrawGalaxyCard();
            }
        }
    }
}