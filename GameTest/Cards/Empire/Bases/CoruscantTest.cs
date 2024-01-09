using GameTest.Cards.Bases.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Bases
{
    [TestFixture]
    public class CoruscantTest : EmpireAvailableBaseTest, IHasAtStartOfTurnTest
    {
        public override int Id => 129;

        public void AssertAfterStartOfTurn() 
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.First().Action, Is.EqualTo(Action.GalacticRule));
            That(Game.KnowsTopCardOfDeck[Faction.empire], Is.EqualTo(2));
            That(Game.KnowsTopCardOfDeck[Faction.rebellion], Is.EqualTo(0));

            PlayableCard card1 = Game.GalaxyDeck.BaseList.ElementAt(0);
            PlayableCard card2 = Game.GalaxyDeck.BaseList.ElementAt(1);

            Game.ApplyAction(Action.GalacticRule, card1.Id);
            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(card1.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(Game.GalaxyDeck.BaseList.ElementAt(0), Is.EqualTo(card2));
        }

        [Test]
        public void TestPickOtherCard() 
        {
            ((IHasAtStartOfTurnTest) this).TriggerStartOfTurn();
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.First().Action, Is.EqualTo(Action.GalacticRule));
            That(Game.KnowsTopCardOfDeck[Faction.empire], Is.EqualTo(2));
            That(Game.KnowsTopCardOfDeck[Faction.rebellion], Is.EqualTo(0));

            PlayableCard card1 = Game.GalaxyDeck.BaseList.ElementAt(0);
            PlayableCard card2 = Game.GalaxyDeck.BaseList.ElementAt(1);

            Game.ApplyAction(Action.GalacticRule, card2.Id);
            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(card2.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(Game.GalaxyDeck.BaseList.ElementAt(0), Is.EqualTo(card1));
        }
    }
}