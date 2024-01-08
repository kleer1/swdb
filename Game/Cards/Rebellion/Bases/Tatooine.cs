using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Game.Cards.Rebellion.Bases
{
    public class Tatooine : Base, IHasOnReveal
    {
        public Tatooine(int id, SWDBGame game) :
            base(id, Faction.rebellion, "Tatooine", CardLocation.RebelAvailableBases, (IList<Card>) game.Rebel.AvailableBases,
                game, game.Rebel, 14) {}

        public void ApplyOnReveal() 
        {
            if (Game.GalaxyDiscard.Where(c => c.Faction == Faction.rebellion).Any()) 
            {
                Game.PendingActions.Add(PendingAction.Of(Action.ANewHope1));
            }
        }
    }
}