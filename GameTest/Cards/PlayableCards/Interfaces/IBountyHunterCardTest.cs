using Game.Cards.Common.Models.Interface;
using GameTest.Cards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.IPlayableCards.Interfaces
{
    public interface IBountyHunterCardTest : IBaseIPlayableCard
    {
        [Test]
        void TestBountyHunterReward() 
        {
            Card.MoveToInPlay();
            IPlayableCard target;
            if (Game.CurrentPlayersAction == Faction.empire) 
            {
                // add y-wing id = 50
                target = (IPlayableCard) Game.CardMap[50];
            } else 
            {
                // add tie fighter id = 10
                target = (IPlayableCard) Game.CardMap[10];
            }
            target.MoveToGalaxyRow();
            VerifyPreBounty();
            Game.ApplyAction(SWDB.Game.Actions.Action.AttackCenterRow, target.Id);
            Game.ApplyAction(SWDB.Game.Actions.Action.SelectAttacker, Card.Id);
            Game.ApplyAction(SWDB.Game.Actions.Action.ConfirmAttackers);
            VerifyBountyHunterReward();
        }

        void VerifyPreBounty() 
        {

        }

        void VerifyBountyHunterReward();
    }
}