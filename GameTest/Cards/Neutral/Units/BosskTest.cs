using GameTest.Cards.PlayableCards.Interfaces;
using SWDB.Game.Common;

namespace GameTest.Cards.Neutral.Units
{
    [TestFixture]
    public class BosskTest : NeutralPlayableCardTest, IBountyHunterCardTest
    {
        public override int Id => 107;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,3, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public void VerifyBountyHunterReward()
        {
            AssertForceIncreasedBy(Faction.empire, 1);
        }
    }
}