using Game.Cards.Common.Models.Interface;
using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Units
{
    [TestFixture]
    public class JynErsoTest : RebelTargetableCardTest, IHasAbilityCardTest
    {
        public override int Id => 67;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,4, 0);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            AssertForceIncreasedBy(Faction.empire, 3);
        }

        public void SetupAbility() 
        {
            GetPlayer().AddForce(1);
        }

        public void VerifyAbility()
        {
            That(Game.CanSeeOpponentsHand, Is.EqualTo(true));
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.JynErsoTopDeck));

            IPlayableCard? card1 = GetPlayer().Opponent?.Hand.BaseList.ElementAt(0);
            Game.ApplyAction(Action.JynErsoTopDeck, card1?.Id);

            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(card1, Is.EqualTo(Game.Empire.Deck.ElementAt(0)));
        }

        [Test]
        public void TestNotForceWithYou() 
        {
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);
            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(Game.CanSeeOpponentsHand, Is.EqualTo(true));
        }
    }
}