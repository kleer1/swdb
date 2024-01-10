using GameTest.Cards.PlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Units
{
    [TestFixture]
    public class TieInterceptorTest : EmpireTargetableCardTest, IHasAbilityCardTest
    {
        public override int Id => 20;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,3, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            AssertGameState(Game.Rebel,0, 3);
            AssertNoForceChange();
        }

        public void SetupAbility() 
        {
            SetUpAbility(EMPIRE_GALAXY_CARD);
        }

        public void VerifyAbility()
        {
            PlayableCard playableCard = (PlayableCard) Game.CardMap[BaseTest.EMPIRE_GALAXY_CARD];
            That(playableCard.Location, Is.EqualTo(CardLocation.GalaxyDeck));
            That(playableCard, Is.EqualTo(Game.GalaxyDeck.ElementAt(0)));
            That(GetPlayer().Hand, Has.Count.EqualTo(6));
            That(Game.KnowsTopCardOfDeck[Faction.empire], Is.EqualTo(1));
            That(Game.KnowsTopCardOfDeck[Faction.rebellion], Is.EqualTo(1));
        }

        [Test]
        public void TestRebelCardOnTop() 
        {
            SetUpAbility(REBEL_GALAXY_CARD);
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);
            PlayableCard playableCard = (PlayableCard) Game.CardMap[BaseTest.REBEL_GALAXY_CARD];
            That(playableCard.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(GetPlayer().Hand, Has.Count.EqualTo(5));
            That(Game.KnowsTopCardOfDeck[Faction.empire], Is.EqualTo(0));
            That(Game.KnowsTopCardOfDeck[Faction.rebellion], Is.EqualTo(0));
        }

        [Test]
        public void TestNeutralCardOnTop() 
        {
            SetUpAbility(NEUTRAL_GALAXY_CARD);
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);
            PlayableCard playableCard = (PlayableCard) Game.CardMap[BaseTest.NEUTRAL_GALAXY_CARD];
            That(playableCard.Location, Is.EqualTo(CardLocation.GalaxyDeck));
            That(playableCard, Is.EqualTo(Game.GalaxyDeck.ElementAt(0)));
            That(GetPlayer().Hand, Has.Count.EqualTo(5));
            That(Game.KnowsTopCardOfDeck[Faction.empire], Is.EqualTo(1));
            That(Game.KnowsTopCardOfDeck[Faction.rebellion], Is.EqualTo(1));
        }

        private void SetUpAbility(int id) 
        {
            PlayableCard playableCard = (PlayableCard) Game.CardMap[id];
            playableCard.MoveToTopOfGalaxyDeck();
        }
    }
}