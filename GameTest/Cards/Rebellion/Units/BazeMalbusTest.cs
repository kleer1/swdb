using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Units
{
    [TestFixture]
    public class BazeMalbusTest : RebelTargetableCardTest
    {
        public override int Id => 65;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,2, 0);
            AssertNoForceChange();

            Game.Empire.CurrentBase?.AddDamage(Game.Empire.CurrentBase.HitPoints);
            Game.Empire.AvailableBases.BaseList.ElementAt(0).MakeCurrentBase();

            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,3, 0);
            AssertNoForceChange();

            Game.Empire.CurrentBase?.AddDamage(Game.Empire.CurrentBase.HitPoints);
            Game.Empire.AvailableBases.BaseList.ElementAt(0).MakeCurrentBase();

            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,4, 0);
            AssertNoForceChange();

            Game.Empire.CurrentBase?.AddDamage(Game.Empire.CurrentBase.HitPoints);
            Game.Empire.AvailableBases.BaseList.ElementAt(0).MakeCurrentBase();

            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,5, 0);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            AssertForceIncreasedBy(Faction.empire, 1);
        }
    }
}