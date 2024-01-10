namespace GameTest.Cards.Rebellion.Ships
{
    [TestFixture]
    public class MonCalamariCruiserTest : RebelIPlayableCardTest
    {
        public override int Id => 78;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,3, 0);
            AssertNoForceChange();
        }
    }
}