using GameTest.Cards.PlayableCards.Interfaces;
using SWDB.Game;

namespace GameTest.Cards.Neutral.Units
{
    [TestFixture]
    public class TwilekSmugglerTest : NeutralPlayableCardTest, IHasAbilityCardTest
    {
        public override int Id => 98;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 3);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public void VerifyAbility()
        {
            That(Game.StaticEffects, Has.Count.EqualTo(1));
            That(Game.StaticEffects, Does.Contain(StaticEffect.BuyNextToTopOfDeck));
            That(Game.PendingActions, Has.Count.EqualTo(0));
        }
    }
}