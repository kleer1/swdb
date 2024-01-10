using GameTest.Cards.Bases.Interfaces;
using SWDB.Game;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Bases
{
    [TestFixture]
    public class CorelliaTest : EmpireAvailableBaseTest, IHasOnRevealTest
    {
        public override int Id => 123;

        public void AssertAfterChooseBase()
        {
            
            Assert.Multiple(() =>
            {
                That(Game.StaticEffects, Has.Count.EqualTo(2));
                That(Game.StaticEffects, Is.EquivalentTo(new List<StaticEffect> {
                    StaticEffect.NextFactionOrNeutralPurchaseIsFree,
                    StaticEffect.BuyNextToHand}));
                That(Game.PendingActions, Has.Count.EqualTo(1));
                That(Game.PendingActions.First().Action, Is.EqualTo(SWDB.Game.Actions.Action.PurchaseCard));
            });
           

            var card = Game.GalaxyRow.BaseList.First();
            var empire = (PlayableCard) Game.CardMap[BaseTest.EMPIRE_GALAXY_CARD];
            if (empire.Location != CardLocation.GalaxyRow)
            {
                card.MoveToTopOfGalaxyDeck();
                empire.MoveToGalaxyRow();
            }
            That(Game.GalaxyRow, Has.Count.EqualTo(6));

            Game.ApplyAction(SWDB.Game.Actions.Action.PurchaseCard, empire.Id);
            Assert.Multiple(() =>
            {
                That(empire.Location, Is.EqualTo(CardLocationHelper.GetHand(GetPlayer().Faction)));
                That(Game.StaticEffects, Has.Count.EqualTo(0));
                That(Game.PendingActions, Has.Count.EqualTo(0));
            });
        }

        [Test]
        public void TestPurchaseOfNeutral()
        {
            ((IHasOnRevealTest)this).ChooseBase();

            That(Game.StaticEffects, Has.Count.EqualTo(2));
            That(Game.StaticEffects, Is.EquivalentTo(new List<StaticEffect> {
                StaticEffect.NextFactionOrNeutralPurchaseIsFree,
                StaticEffect.BuyNextToHand}));
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.First().Action, Is.EqualTo(SWDB.Game.Actions.Action.PurchaseCard));

            var card = Game.GalaxyRow.BaseList.First();
            var neutral = (PlayableCard)Game.CardMap[BaseTest.NEUTRAL_GALAXY_CARD];
            if (neutral.Location != CardLocation.GalaxyRow)
            {
                card.MoveToTopOfGalaxyDeck();
                neutral.MoveToGalaxyRow();
            }
            That(Game.GalaxyRow, Has.Count.EqualTo(6));

            Game.ApplyAction(SWDB.Game.Actions.Action.PurchaseCard, neutral.Id);
            That(neutral.Location, Is.EqualTo(CardLocationHelper.GetHand(GetPlayer().Faction)));
            That(Game.StaticEffects, Has.Count.EqualTo(0));
            That(Game.PendingActions, Has.Count.EqualTo(0));
        }
    }
}