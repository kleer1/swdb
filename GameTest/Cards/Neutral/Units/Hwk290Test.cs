using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Common;

namespace GameTest.Cards.Neutral.Units
{
    [TestFixture]
    public class Hwk290Test : NeutralIPlayableCardTest, IHasAbilityCardTest
    {
        public override int Id => 102;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 4);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public void SetupAbility() 
        {
            GetPlayer().CurrentBase?.AddDamage(5);
        }

        public void VerifyAbility()
        {
            That(Card.Location, Is.EqualTo(CardLocation.Exile));
            That(GetPlayer().CurrentBase?.CurrentDamage, Is.EqualTo(1));
        }
    }
}