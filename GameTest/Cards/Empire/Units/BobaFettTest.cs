using GameTest.Cards.PlayableCards.Interfaces;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Units
{
    [TestFixture]
    public class BobaFettTest : EmpireTargetableCardTest, IBountyHunterCardTest
    {
        public override int Id => 29;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,5, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            That(Game.Rebel.Resources, Is.EqualTo(3));
            AssertForceIncreasedBy(Faction.rebellion, 2);
        }

        public void VerifyPreBounty() 
        {
            That(GetPlayer().Hand, Has.Count.EqualTo(5));
        }

        public void VerifyBountyHunterReward()
        {
            That(GetPlayer().Hand, Has.Count.EqualTo(6));
        }
    }
}