using SWDB.Game.Cards.Empire.Units;

namespace GameTest.Cards.Empire.Ships
{
    public class ImperialCarrierTest : EmpirePlayableCardTest
    {
        public override int Id => 36;

        protected override void PrePlaySetup() 
        {
            MoveToInPlay(typeof(TieFighter), GetPlayer(), 2);
        }

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire, 6, 3);
            AssertGameState(Game.Rebel, 0, 0);
            AssertNoForceChange();

            Game.ApplyAction(Action.PassTurn);
            AssertGameState(Game.Empire, 0, 0);
            AssertGameState(Game.Rebel, 0, 0);
            AssertNoForceChange();

            Game.ApplyAction(Action.PassTurn);
            AssertGameState(Game.Empire, 0, 3);
            AssertGameState(Game.Rebel, 0, 0);
            AssertNoForceChange();

            MoveToInPlay(typeof(TieBomber), GetPlayer(), 2);
            AssertGameState(Game.Empire, 6, 3);
            AssertGameState(Game.Rebel, 0, 0);
            AssertNoForceChange();

            Card.MoveToDiscard();
            AssertGameState(Game.Empire, 4, 3);
            AssertGameState(Game.Rebel, 0, 0);
            AssertNoForceChange();
        }
    }
}