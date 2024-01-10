using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Units
{
    [TestFixture]
    public class DeathTrooperTest : EmpireTargetableCardTest
    {
        public override int Id => 14;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire, 3, 0);
            AssertGameState(Game.Rebel, 0, 0);
            AssertNoForceChange();

            Game.Empire.AddForce(1);
            AssertGameState(Game.Empire, 5, 0);
        }

        public override void AssertReward()
        {
            AssertForceIncreasedBy(Faction.rebellion, 2);
        }
    }
}