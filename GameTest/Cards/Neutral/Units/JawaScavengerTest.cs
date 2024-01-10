using GameTest.Cards.PlayableCards.Interfaces;
using SWDB.Game;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Neutral.Units
{
    [TestFixture]
    public class JawaScavengerTest : NeutralPlayableCardTest, IHasAbilityCardTest
    {
        private PlayableCard? neutral;
        private PlayableCard? empire;
        private PlayableCard? rebel;
        private PlayableCard? empire2;
        int startingResources = 2;

        public override int Id => 92;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 2);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public void SetupAbility() 
        {
            neutral = (PlayableCard) Game.CardMap[NEUTRAL_GALAXY_CARD];
            neutral.MoveToGalaxyDiscard();
            empire = (PlayableCard) Game.CardMap[EMPIRE_GALAXY_CARD];
            empire.MoveToGalaxyDiscard();
            rebel = (PlayableCard) Game.CardMap[REBEL_GALAXY_CARD];
            rebel.MoveToGalaxyDiscard();
            empire2 = (PlayableCard) Game.CardMap[EMPIRE_GALAXY_CARD + 1];
            empire2.MoveToGalaxyRow();
        }

        public void VerifyAbility()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.PurchaseCard));
            That(Game.StaticEffects, Has.Count.EqualTo(1));
            That(Game.StaticEffects.ElementAt(0), Is.EqualTo(StaticEffect.PurchaseFromDiscard));

            // buy empire card
            Game.ApplyAction(Action.PurchaseCard, empire?.Id);

            That(empire?.Location, Is.EqualTo(CardLocation.EmpireDiscard));
            That(GetPlayer().Resources, Is.EqualTo(startingResources - empire?.Cost));
            That(rebel?.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(neutral?.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(empire2?.Location, Is.EqualTo(CardLocation.GalaxyRow));
            That(Game.StaticEffects, Has.Count.EqualTo(0));
            That(Game.PendingActions, Has.Count.EqualTo(0));
        }

        [Test]
        public void TestPurchaseNeutralCard() 
        {
            SetupAbility();
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);

            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.PurchaseCard));
            That(Game.StaticEffects, Has.Count.EqualTo(1));
            That(Game.StaticEffects.ElementAt(0), Is.EqualTo(StaticEffect.PurchaseFromDiscard));

            // buy neutral card
            Game.ApplyAction(Action.PurchaseCard, neutral?.Id);

            That(neutral?.Location, Is.EqualTo(CardLocation.EmpireDiscard));
            That(GetPlayer().Resources, Is.EqualTo(startingResources - neutral?.Cost));
            That(rebel?.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(empire?.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(empire2?.Location, Is.EqualTo(CardLocation.GalaxyRow));
            That(Game.StaticEffects, Has.Count.EqualTo(0));
            That(Game.PendingActions, Has.Count.EqualTo(0));
        }

        [Test]
        public void TestCantBuyRebel() 
        {
            SetupAbility();
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);

            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.PurchaseCard));
            That(Game.StaticEffects, Has.Count.EqualTo(1));
            That(Game.StaticEffects.ElementAt(0), Is.EqualTo(StaticEffect.PurchaseFromDiscard));

            // buy neutral card
            Game.ApplyAction(Action.PurchaseCard, rebel?.Id);

            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.PurchaseCard));
            That(Game.StaticEffects, Has.Count.EqualTo(1));
            That(Game.StaticEffects.ElementAt(0), Is.EqualTo(StaticEffect.PurchaseFromDiscard));

            That(neutral?.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(rebel?.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(empire?.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(empire2?.Location, Is.EqualTo(CardLocation.GalaxyRow));
        }

        [Test]
        public void TestCantBuyFromCenter() 
        {
            SetupAbility();
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);

            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.PurchaseCard));
            That(Game.StaticEffects, Has.Count.EqualTo(1));
            That(Game.StaticEffects.ElementAt(0), Is.EqualTo(StaticEffect.PurchaseFromDiscard));

            // buy center card
            Game.ApplyAction(Action.PurchaseCard, empire2?.Id);

            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.PurchaseCard));
            That(Game.StaticEffects, Has.Count.EqualTo(1));
            That(Game.StaticEffects.ElementAt(0), Is.EqualTo(StaticEffect.PurchaseFromDiscard));

            That(neutral?.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(rebel?.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(empire?.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(empire2?.Location, Is.EqualTo(CardLocation.GalaxyRow));
        }

        [Test]
        public void TestNotActive() 
        {
            rebel = (PlayableCard) Game.CardMap[REBEL_GALAXY_CARD];
            rebel.MoveToGalaxyDiscard();
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);
            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(Game.StaticEffects, Has.Count.EqualTo(0));
        }
    }
}