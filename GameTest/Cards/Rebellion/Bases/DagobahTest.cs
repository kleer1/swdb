using Game.Cards.Common.Models.Interface;
using GameTest.Cards.Bases.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Bases
{
    [TestFixture]
    public class DagobahTest : RebelAvailableBaseTest, IHasOnRevealTest
    {
        public override int Id => 133;

        public void AssertAfterChooseBase()
        {
            GetPlayer().Hand.BaseList.ElementAt(0).MoveToDiscard();

            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ExileCard));

            IPlayableCard card1 = GetPlayer().Hand.BaseList.ElementAt(0);
            IPlayableCard card2 = GetPlayer().Hand.BaseList.ElementAt(1);
            IPlayableCard card3 = GetPlayer().Discard.BaseList.ElementAt(0);

            Game.ApplyAction(Action.ExileCard, card1.Id);
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ExileCard));
            That(card1.Location, Is.EqualTo(CardLocation.Exile));
            That(card2.Location, Is.EqualTo(CardLocationHelper.GetHand(GetPlayer().Faction)));
            That(card3.Location, Is.EqualTo(CardLocationHelper.GetDiscard(GetPlayer().Faction)));

            Game.ApplyAction(Action.ExileCard, card2.Id);
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ExileCard));
            That(card1.Location, Is.EqualTo(CardLocation.Exile));
            That(card2.Location, Is.EqualTo(CardLocation.Exile));
            That(card3.Location, Is.EqualTo(CardLocationHelper.GetDiscard(GetPlayer().Faction)));

            Game.ApplyAction(Action.ExileCard, card3.Id);
            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(card1.Location, Is.EqualTo(CardLocation.Exile));
            That(card2.Location, Is.EqualTo(CardLocation.Exile));
            That(card3.Location, Is.EqualTo(CardLocation.Exile));
        }
    }
}