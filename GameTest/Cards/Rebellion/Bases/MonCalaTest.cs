using GameTest.Cards.Bases.Interfaces;
using SWDB.Game;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Bases
{
    [TestFixture]
    public class MonCalaTest : RebelAvailableBaseTest, IHasOnRevealTest
    {
        public override int Id => 132;

        public void AssertAfterChooseBase()
        {
            That(Game.StaticEffects, Has.Count.EqualTo(2));
            That(Game.StaticEffects, Is.EquivalentTo( new List<StaticEffect> {
                StaticEffect.NextFactionOrNeutralPurchaseIsFree,
                StaticEffect.BuyNextToHand
            }));
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.PurchaseCard));

            PlayableCard card = Game.GalaxyRow.BaseList.ElementAt(0);
            PlayableCard rebel = (PlayableCard) Game.CardMap[BaseTest.REBEL_GALAXY_CARD];
            if (rebel.Location != CardLocation.GalaxyRow) {
                card.MoveToTopOfGalaxyDeck();
                rebel.MoveToGalaxyRow();
            }
            That(Game.GalaxyRow, Has.Count.EqualTo(6));

            Game.ApplyAction(Action.PurchaseCard, rebel.Id);
            That(rebel.Location, Is.EqualTo(CardLocationHelper.GetHand(GetPlayer().Faction)));
            That(Game.StaticEffects, Has.Count.EqualTo(0));
            That(Game.PendingActions, Has.Count.EqualTo(0));
        }

        [Test]
        public void TestPurchaseOfNeutral() 
        {
            ((IHasOnRevealTest) this).ChooseBase();

            That(Game.StaticEffects, Has.Count.EqualTo(2));
            That(Game.StaticEffects, Is.EquivalentTo( new List<StaticEffect> {
                StaticEffect.NextFactionOrNeutralPurchaseIsFree,
                StaticEffect.BuyNextToHand
            }));
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.PurchaseCard));

            PlayableCard card = Game.GalaxyRow.BaseList.ElementAt(0);
            PlayableCard neutral = (PlayableCard) Game.CardMap[BaseTest.NEUTRAL_GALAXY_CARD];
            if (neutral.Location != CardLocation.GalaxyRow) {
                card.MoveToTopOfGalaxyDeck();
                neutral.MoveToGalaxyRow();
            }

            That(Game.GalaxyRow, Has.Count.EqualTo(6));

            Game.ApplyAction(Action.PurchaseCard, neutral.Id);

            That(neutral.Location, Is.EqualTo(CardLocationHelper.GetHand(GetPlayer().Faction)));
            That(Game.StaticEffects, Has.Count.EqualTo(0));
            That(Game.PendingActions, Has.Count.EqualTo(0));
        }
    }
}