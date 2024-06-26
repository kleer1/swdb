using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Game.Cards.Empire.Bases
{
    public class Coruscant : Base, IHasAtStartOfTurn
    {
        public Coruscant(int id, SWDBGame game) :
            base(id, Faction.empire, "Coruscant", CardLocation.EmpireAvailableBases,
                game.Empire.AvailableBases, game, game.Empire, 16) {}
        
        public void ApplyAtStartOfTurn() 
        {
            if (Game.GalaxyDeck.Count >= 2) 
            {
                Game.KnowsTopCardOfDeck[Faction.empire] = 2;
                Game.PendingActions.Add(PendingAction.Of(Action.GalacticRule));
            }
        }
    }
}