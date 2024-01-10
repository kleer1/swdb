using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Common;

namespace GameTest.Cards.Neutral.Units
{
    [TestFixture]
    public class KelDorMysticTest : NeutralIPlayableCardTest, IHasAbilityCardTest
    {
        public override int Id => 96;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertForceIncreasedBy(Faction.empire, 2);
        }

        public void VerifyAbility()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ExileCard));
            That(Card.Location, Is.EqualTo(CardLocation.Exile));
        }
    }
}