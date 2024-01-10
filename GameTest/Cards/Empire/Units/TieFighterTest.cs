using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Cards.Empire.Ships;

namespace GameTest.Cards.Empire.Units
{
    [TestFixture]
    public class TieFighterTest : EmpireTargetableCardTest, IHasAbilityCardTest
    {
        public override int Id => 11;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,2, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            That(Game.Rebel.Resources, Is.EqualTo(1));
        }

        public void SetupAbility() 
        {
            MoveToInPlay(typeof(StarDestroyer), GetPlayer());
        }

        public void VerifyAbility()
        {
            That(GetPlayer().Hand, Has.Count.EqualTo(6));
        }

        [Test]
        public void TestAbilityWithNoShip() 
        {
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);
            That(GetPlayer().Hand, Has.Count.EqualTo(5));
        }
    }
}