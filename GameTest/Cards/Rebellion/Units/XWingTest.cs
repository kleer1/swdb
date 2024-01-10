using GameTest.Cards.PlayableCards.Interfaces;

namespace GameTest.Cards.Rebellion.Units
{
    [TestFixture]
    public class XWingTest : RebelTargetableCardTest, IHasAbilityCardTest
    {
        public override int Id => 58;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,3, 0);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            AssertGameState(Game.Empire,0, 3);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public void SetupAbility() 
        {
            GetPlayer().AddForce(1);
        }

        public void VerifyAbility()
        {
            That(GetPlayer().Hand, Has.Count.EqualTo(6));
        }
    }
}