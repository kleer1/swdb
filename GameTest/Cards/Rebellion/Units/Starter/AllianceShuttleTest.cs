namespace GameTest.Cards.Rebellion.Units.Starter
{
    [TestFixture]
    public class AllianceShuttleTest : RebelPlayableCardTest
    {
        public override int Id => 40;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,0, 1);
            AssertNoForceChange();
        }
    }
}