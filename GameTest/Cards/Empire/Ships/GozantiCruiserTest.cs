using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Ships
{
    public class GozantiCruiserTest : EmpireIPlayableCardTest, IHasAbilityCardTest
    {
        int handCard = -1;
        int deckCard = -1;

        public override int Id => 33;

        public void SetupAbility() 
        {
            handCard = GetPlayer().Hand.ElementAt(0).Id;
            deckCard = GetPlayer().Deck.ElementAt(0).Id;
        }

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 2);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();

            Game.ApplyAction(Action.PassTurn);
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();

            Game.ApplyAction(Action.PassTurn);
            AssertGameState(Game.Empire,0, 2);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public void VerifyAbility()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.DiscardFromHand));

            Game.ApplyAction(Action.DiscardFromHand, handCard);
            Card card1 = Game.CardMap[handCard];
            That(card1.Location, Is.EqualTo(CardLocation.EmpireDiscard));

            Card card2 = Game.CardMap[deckCard];
            That(card2.Location, Is.EqualTo(CardLocation.EmpireHand));

            That(GetPlayer().Hand, Has.Count.EqualTo(5));
        }
    }
}