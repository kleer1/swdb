namespace GameTest.Cards.Neutral.Ships
{
    [TestFixture]
    public class BlockadeRunnerTest : NeutralPlayableCardTest
    {
        public override int Id => 115;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,1, 1);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }
    }
}