using Game.Cards.Common.Models.Interface;
using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Units
{
    [TestFixture]
    public class ScoutTrooperTest : EmpireTargetableCardTest, IHasAbilityCardTest
    {
        private static readonly int REBEL_CARD_Id = 40;

        public override int Id => 17;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 2);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ExileCard));

            IPlayableCard card1 = (IPlayableCard) Game.CardMap[REBEL_CARD_Id];
            card1.MoveToDiscard();

            Game.ApplyAction(Action.ExileCard, REBEL_CARD_Id);
            That(card1.Location, Is.EqualTo(CardLocation.Exile));
            IsNull(card1.Owner);
            That(card1.CardList, Is.EqualTo(Game.ExiledCards));
            That(Game.PendingActions, Has.Count.EqualTo(0));
        }

        public void SetupAbility() {
            IPlayableCard card1 = (IPlayableCard) Game.CardMap[28];
            card1.MoveToTopOfGalaxyDeck();
        }

        public void VerifyAbility()
        {
            That(Game.KnowsTopCardOfDeck[Faction.empire], Is.EqualTo(1));
            That(Game.KnowsTopCardOfDeck[Faction.rebellion], Is.EqualTo(1));
            AssertForceIncreasedBy(Faction.empire, 1);
        }

        [Test]
        public void testRebelCardOnTop() 
        {
            IPlayableCard card1 = (IPlayableCard) Game.CardMap[50];
            card1.MoveToTopOfGalaxyDeck();
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);
            That(card1.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(Game.KnowsTopCardOfDeck[Faction.empire], Is.EqualTo(0));
            That(Game.KnowsTopCardOfDeck[Faction.rebellion], Is.EqualTo(0));
            AssertNoForceChange();
        }

        [Test]
        public void testNeutralCardOnTop() 
        {
            IPlayableCard card1 = (IPlayableCard) Game.CardMap[90];
            card1.MoveToTopOfGalaxyDeck();
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);
            That(card1.Location, Is.EqualTo(CardLocation.GalaxyDeck));
            That(Game.KnowsTopCardOfDeck[Faction.empire], Is.EqualTo(1));
            That(Game.KnowsTopCardOfDeck[Faction.rebellion], Is.EqualTo(1));
            AssertNoForceChange();
        }
    }
}