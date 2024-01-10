using GameTest.Cards.IPlayableCards.Interfaces;

namespace GameTest.Cards.Neutral.Units
{
    [TestFixture]
    public class Ig88Test : NeutralIPlayableCardTest, IBountyHunterCardTest
    {
        public override int Id => 109;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,5, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public void VerifyBountyHunterReward()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ExileCard));
        }
    }
}