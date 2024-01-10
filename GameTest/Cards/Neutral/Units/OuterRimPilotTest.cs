using Game.Cards.Common.Models.Interface;
using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Neutral.Units
{
    [TestFixture]
    public class OuterRimPilotTest : NeutralIPlayableCardTest, IHasAbilityCardTest
    {
        public override int Id => 80;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 2);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public void VerifyAbility()
        {
            That(Card.Location, Is.EqualTo(CardLocation.Exile));
            AssertForceIncreasedBy(Faction.empire, 1);
        }

        [Test]
        public void TestCanBePurchased() 
        {
            GetPlayer().AddResources(3);
            IPlayableCard orp = Game.OuterRimPilots.BaseList.ElementAt(0);

            Game.ApplyAction(Action.PurchaseCard, orp.Id);

            That(orp.Location, Is.EqualTo(CardLocation.EmpireDiscard));
            That(GetPlayer().Resources, Is.EqualTo(1));
        }
    }
}