using SWDB.Cards.Common.Models;
using SWDB.Cards.Common.Models.Interface;
using SWDB.Game;
using SWDB.Game.Actions;
using Action = SWDB.Game.Actions.Action;

namespace SWDB.Cards.Rebellion.Units
{
    public class CassianAndor : RebelGalaxyUnit, IBountyHunter
    {
        public CassianAndor(int id, SWDBGame game) :
            base(id, 5, 5, 0, 0, "Cassian Andor", new List<Trait>{ Trait.trooper }, true, game) {}

        public void ReceiveBounty() 
        {
            if (Owner?.Opponent?.Hand.Any() ?? false) 
            {
                Game.PendingActions.Add(PendingAction.Of(Action.DiscardFromHand, true));
            }
        }

        public override int GetTargetValue() 
        {
            return 5;
        }

        public override void ApplyReward() 
        {
            Game.Empire.AddResources(3);
            Game.Empire.AddForce(2);
        }
    }
}