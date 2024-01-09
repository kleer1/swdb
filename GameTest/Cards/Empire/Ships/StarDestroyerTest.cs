namespace GameTest.Cards.Empire.Ships
{
    public class StarDestroyerTest : EmpirePlayableCardTest
    {
        public override int Id => 39;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire, 4, 0);
            AssertGameState(Game.Rebel, 0, 0);
            AssertNoForceChange();

            Game.ApplyAction(Action.PassTurn);
            AssertGameState(Game.Empire, 4, 0);
            AssertGameState(Game.Rebel, 0, 0);
            AssertNoForceChange();

            Game.ApplyAction(Action.PassTurn);
            AssertGameState(Game.Empire, 4, 0);
            AssertGameState(Game.Rebel, 0, 0);
            AssertNoForceChange();
        }
    }
}