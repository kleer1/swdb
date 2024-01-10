using GameTest.Cards.PlayableCards.Interfaces;

namespace GameTest.Cards.Neutral.Units
{
    [TestFixture]
    public class DengarTest : NeutralPlayableCardTest, IBountyHunterCardTest
    {
        public override int Id => 108;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,4, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public void VerifyBountyHunterReward()
        {
            // 2 for dengar, 1 for tie fighter
            AssertGameState(GetPlayer(), 0, 3);
        }
    }
}