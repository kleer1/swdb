using Game.Cards.Common.Models.Interface;
using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Neutral.Units
{
    [TestFixture]
    public class QuarrenMercenaryTest : NeutralIPlayableCardTest, IHasOnPurchaseTest
    {
        public override int Id => 104;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,4, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public void BeforePurchase() 
        {
            GetPlayer().AddForce(1);
        }

        public void VerifyAfterPurchase()
        {
            That(Card.Location, Is.EqualTo(CardLocation.EmpireDiscard));

            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ExileCard));

            IPlayableCard card1 = Game.Empire.Hand.BaseList.ElementAt(0);
            Game.ApplyAction(Action.ExileCard, card1.Id);

            That(card1.Location, Is.EqualTo(CardLocation.Exile));
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ExileCard));

            card1 = Game.Empire.Hand.BaseList.ElementAt(0);
            Game.ApplyAction(Action.ExileCard, card1.Id);

            That(card1.Location, Is.EqualTo(CardLocation.Exile));
            That(Game.PendingActions, Has.Count.EqualTo(0));
        }
    }
}