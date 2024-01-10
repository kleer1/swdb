using Game.Cards.Common.Models.Interface;
using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Neutral.Units
{
    [TestFixture]
    public class FangFighterTest : NeutralIPlayableCardTest, IHasOnPurchaseTest
    {
        private IPlayableCard? topCard;

        public override int Id => 100;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,3, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public void BeforePurchase() 
        {
            topCard = GetPlayer().Deck.BaseList.ElementAt(0);
            GetPlayer().AddForce(1);
        }

        public void VerifyAfterPurchase()
        {
            That(Card.Location, Is.EqualTo(CardLocation.EmpireHand));
            That(topCard?.Location, Is.EqualTo(CardLocation.EmpireHand));
            That(GetPlayer().Hand, Has.Count.EqualTo(7));
        }

        [Test]
        public void testForceIsNotWithYou() 
        {
            ((IHasOnPurchaseTest) this).Purchase();
            That(Card.Location, Is.EqualTo(CardLocation.EmpireHand));
            That(GetPlayer().Hand, Has.Count.EqualTo(6));
        }
    }
}