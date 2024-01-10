using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Units
{
    [TestFixture]
    public class YWingTest : RebelTargetableCardTest, IHasAbilityCardTest
    {
        public override int Id => 50;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,2, 0);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            AssertGameState(Game.Empire,0, 1);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public void VerifyAbility()
        {
            That(Card.Location, Is.EqualTo(CardLocation.Exile));
            That(GetPlayer().Opponent?.CurrentBase?.CurrentDamage, Is.EqualTo(2));
        }
    }
}