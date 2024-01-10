namespace GameTest.Cards.Empire.Units.Starter
{
    [TestFixture]
    public class StormtrooperTest : EmpirePlayableCardTest
    {
        public override int Id => 7;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,2, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }
    }
}