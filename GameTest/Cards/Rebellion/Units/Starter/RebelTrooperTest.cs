namespace GameTest.Cards.Rebellion.Units.Starter
{
    [TestFixture]
    public class RebelTrooperTest : RebelPlayableCardTest
    {
        public override int Id => 47;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,2, 0);
            AssertNoForceChange();
        }
    }
}