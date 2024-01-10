using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Units
{
    [TestFixture]
    public class DarthVaderTest : EmpireTargetableCardTest
    {
        public override int Id => 32;

        public void PreAttackSetup() 
        {
            Game.Empire.AddForce(100);
        }

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,10, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertForceIncreasedBy(Faction.empire, 2);

            Game.Rebel.AddForce(100);
            AssertGameState(Game.Empire,6, 0);
        }

        public override void AssertReward()
        {
            That(Game.Rebel.Resources, Is.EqualTo(4));
            That(Game.ForceBalance.Position, Is.EqualTo(4));
        }
    }
}