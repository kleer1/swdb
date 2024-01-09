using GameTest.Cards.Interfaces;

namespace GameTest.Cards.PlayableCards.Interfaces
{
    public interface IHasOnPurchaseTest : IBasePlayableCard
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