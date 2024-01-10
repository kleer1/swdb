using GameTest.Cards.PlayableCards.Interfaces;

namespace GameTest.Cards.Rebellion.Units
{
    [TestFixture]
    public class UWingTest : RebelTargetableCardTest, IHasAbilityCardTest
    {
        public override int Id => 61;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,0, 3);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            AssertGameState(Game.Empire,0, 4);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public void SetupAbility() 
        {
            GetPlayer().AddForce(1);
            GetPlayer().CurrentBase?.AddDamage(5);
        }

        public void VerifyAbility()
        {
            That(GetPlayer().CurrentBase?.CurrentDamage, Is.EqualTo(2));
        }
    }
}