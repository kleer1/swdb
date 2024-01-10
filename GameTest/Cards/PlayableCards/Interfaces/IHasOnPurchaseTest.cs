using GameTest.Cards.Interfaces;

namespace GameTest.Cards.IPlayableCards.Interfaces
{
    public interface IHasOnPurchaseTest : IBaseIPlayableCard
    {
        [Test]
        void TestOnPurchase()
        {
            BeforePurchase();
            Purchase();
            VerifyAfterPurchase();
        }

        void BeforePurchase() 
        {

        }

        void Purchase() 
        {
            Card.MoveToGalaxyRow();
            GetPlayer().AddResources(Card.Cost);
            Game.ApplyAction(SWDB.Game.Actions.Action.PurchaseCard, Card.Id);
        }

        void VerifyAfterPurchase();
    }
}