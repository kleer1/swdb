using Game.Cards.Common.Models.Interface;
using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Neutral.Units
{
    [TestFixture]
    public class LandoCalrissianTest : NeutralIPlayableCardTest, IHasAbilityCardTest
    {
        public override int Id => 110;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,3, 3);
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
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.DiscardFromHand));
            That(Game.CurrentPlayersAction, Is.EqualTo(Faction.rebellion));

            IPlayableCard card1 = Game.Rebel.Hand.BaseList.ElementAt(0);

            Game.ApplyAction(Action.DiscardFromHand, card1.Id);

            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(card1.Location, Is.EqualTo(CardLocation.RebelDiscard));
            That(Game.CurrentPlayersAction, Is.EqualTo(Faction.empire));
        }

        [Test]
        public void TestForceIsNotWithYou() 
        {
            IPlayableCard card1 = GetPlayer().Deck.BaseList.ElementAt(0);

            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);
            That(GetPlayer().Hand, Has.Count.EqualTo(6));
            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(card1.Location, Is.EqualTo(CardLocation.EmpireHand));
        }
    }
}