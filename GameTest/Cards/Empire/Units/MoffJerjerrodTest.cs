using GameTest.Cards.PlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Units
{
    [TestFixture]
    public class MoffJerjerrodTest : EmpireTargetableCardTest, IHasAbilityCardTest
    {
        public override int Id => 28;

        public void SetupAbility() 
        {
            PlayableCard card1 = (PlayableCard) Game.CardMap[29];
            card1.MoveToGalaxyRow();
            PlayableCard card2 = (PlayableCard) Game.CardMap[30];
            card2.MoveToTopOfGalaxyDeck();
            GetPlayer().AddForce(1);
        }

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,2, 2);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            AssertForceIncreasedBy(Faction.rebellion, 3);
        }

        public void VerifyAbility()
        {
            That(Game.KnowsTopCardOfDeck[Faction.empire], Is.EqualTo(1));
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.SwapTopCardOfDeck));

            Game.ApplyAction(Action.SwapTopCardOfDeck, 29);
            PlayableCard card1 = (PlayableCard) Game.CardMap[29];
            That(card1.Location, Is.EqualTo(CardLocation.GalaxyDeck));
            That(card1.CardList, Is.EqualTo(Game.GalaxyDeck));
            That(card1, Is.EqualTo(Game.GalaxyDeck.ElementAt(0)));

            PlayableCard card2 = (PlayableCard) Game.CardMap[30];
            That(card2.Location, Is.EqualTo(CardLocation.GalaxyRow));
            That(card2.CardList, Is.EqualTo(Game.GalaxyRow));
            That(Game.GalaxyRow, Does.Contain(card2));

            That(Game.PendingActions, Has.Count.EqualTo(0));
        }

        [Test]
        public void TestAbilityWithoutForce() 
        {
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);
            That(Game.KnowsTopCardOfDeck[Faction.empire], Is.EqualTo(1));
            That(Game.PendingActions, Has.Count.EqualTo(0));
        }
    }
}