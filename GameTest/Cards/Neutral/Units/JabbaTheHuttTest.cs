using Game.Cards.Common.Models.Interface;
using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Neutral.Units
{
    [TestFixture]
    public class JabbaTheHuttTest : NeutralIPlayableCardTest, IHasAbilityCardTest
    {
        public override int Id => 112;
        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,2, 2);
            AssertGameState(Game.Rebel,0, 0);
            AssertForceIncreasedBy(Faction.empire, 2);
        }

        public void SetupAbility() 
        {
            GetPlayer().Opponent?.AddForce(2);
        }

        public void VerifyAbility()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.JabbaExile));

            IPlayableCard card1 = GetPlayer().Hand.BaseList.ElementAt(0);
            IPlayableCard card2 = GetPlayer().Deck.BaseList.ElementAt(0);

            Game.ApplyAction(Action.JabbaExile, card1.Id);

            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(GetPlayer().Hand, Has.Count.EqualTo(5));
            That(card1.Location, Is.EqualTo(CardLocation.Exile));
            That(card2.Location, Is.EqualTo(CardLocation.EmpireHand));
        }

        [Test]
        public void TestForceIsWithYou() 
        {
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);

            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.JabbaExile));

            IPlayableCard card1 = GetPlayer().Hand.BaseList.ElementAt(0);
            IPlayableCard card2 = GetPlayer().Deck.BaseList.ElementAt(0);
            IPlayableCard card3 = GetPlayer().Deck.BaseList.ElementAt(1);

            Game.ApplyAction(Action.JabbaExile, card1.Id);

            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(GetPlayer().Hand, Has.Count.EqualTo(6));
            That(card1.Location, Is.EqualTo(CardLocation.Exile));
            That(card2.Location, Is.EqualTo(CardLocation.EmpireHand));
            That(card3.Location, Is.EqualTo(CardLocation.EmpireHand));
        }
    }
}