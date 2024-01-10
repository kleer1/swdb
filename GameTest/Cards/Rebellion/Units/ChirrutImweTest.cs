using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Units
{
    [TestFixture]
    public class ChirrutImweTest : RebelTargetableCardTest
    {
        public override int Id => 66;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,2, 0);
            AssertForceIncreasedBy(Faction.rebellion, 2);

            GetPlayer().Opponent?.AddForce(2);
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,0, 0);
        }

        public override void AssertReward()
        {
            AssertForceIncreasedBy(Faction.empire,2);
        }
    }
}