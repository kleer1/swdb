using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Common.Models.Interface;
using SWDB.Game.Common;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Game.Cards.Empire.Bases
{
    public class DeathStar : Base, IHasAbility, IHasAtStartOfTurn
    {
        public DeathStar(int id, SWDBGame game) :
            base(id, Faction.empire, "Death Star", CardLocation.EmpireAvailableBases, game.Empire.AvailableBases,
                game, game.Empire, 16) {}
        
        public override bool AbilityActive() 
        {
            return base.AbilityActive() && Location == CardLocation.EmpireCurrentBase && Owner?.Resources >= 4 &&
                    (!Game.Rebel.ShipsInPlay.Any() || Game.GalaxyRow.Where(c => c is CapitalShip).Any());
        }

        public override void ApplyAbility() 
        {
            base.ApplyAbility();
            Game.PendingActions.Add(PendingAction.Of(Action.FireWhenReady));
        }

        public void ApplyAtStartOfTurn() 
        {
            AbilityUsed = false;
        }
    }
}