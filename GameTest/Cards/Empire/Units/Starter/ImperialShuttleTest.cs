namespace GameTest.Cards.Empire.Units.Starter
{
    [TestFixture]
    public class ImperialShuttleTest : EmpireIPlayableCardTest
    {
        public override int Id => 0;

        public override void AssertAfterPlay() 
        {
            AssertGameState(Game.Empire,0, 1);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }
    }
}