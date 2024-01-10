using Game.Cards.Common.Models.Interface;
using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Units
{
    [TestFixture]
    public class CassianAndorTest : RebelTargetableCardTest, IBountyHunterCardTest
    {
        public override int Id => 70;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,5, 0);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            AssertGameState(Game.Empire,0, 3);
            AssertGameState(Game.Rebel,0, 0);
            AssertForceIncreasedBy(Faction.empire, 2);
        }

        public void VerifyBountyHunterReward()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.DiscardFromHand));
            That(Game.CurrentPlayersAction, Is.EqualTo(Faction.empire));

            // can't decline
            Game.ApplyAction(Action.DeclineAction);
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.DiscardFromHand));
            That(Game.CurrentPlayersAction, Is.EqualTo(Faction.empire));

            IPlayableCard? card1 = GetPlayer().Opponent?.Hand.BaseList.ElementAt(0);
            Game.ApplyAction(Action.DiscardFromHand, card1?.Id);

            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(Game.CurrentPlayersAction, Is.EqualTo(Faction.rebellion));
            That(card1?.Location, Is.EqualTo(CardLocationHelper.GetDiscard(GetPlayer().Opponent?.Faction ?? Faction.neutral)));
        }
    }
}