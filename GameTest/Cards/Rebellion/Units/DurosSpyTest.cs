using GameTest.Cards.PlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Units
{
    [TestFixture]
    public class DurosSpyTest : RebelTargetableCardTest, IHasAbilityCardTest
    {
        public override int Id => 54;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,0, 2);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ExileCard));

            PlayableCard? card1 = GetPlayer().Opponent?.Hand.BaseList.ElementAt(0);

            Game.ApplyAction(Action.ExileCard, card1?.Id);
            That(card1?.Location, Is.EqualTo(CardLocation.Exile));
            That(card1?.Owner, Is.Null);
            That(card1?.CardList, Is.EqualTo(Game.ExiledCards));
            That(Game.PendingActions, Has.Count.EqualTo(0));
        }

        public void VerifyAbility()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.DurosDiscard));
            That(Game.CurrentPlayersAction, Is.EqualTo(Faction.empire));

            PlayableCard? card1 = GetPlayer().Opponent?.Hand.BaseList.ElementAt(0);
            Game.ApplyAction(Action.ExileCard, card1?.Id);

            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(Game.CurrentPlayersAction, Is.EqualTo(Faction.rebellion));
            That(card1?.Location, Is.EqualTo(CardLocationHelper.GetDiscard(GetPlayer().Opponent?.Faction ?? Faction.neutral)));

            AssertNoForceChange();
        }

        [Test]
        public void TestNoDiscard() 
        {
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);

            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.DurosDiscard));
            That(Game.CurrentPlayersAction, Is.EqualTo(Faction.empire));

            Game.ApplyAction(Action.DeclineAction);

            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(Game.CurrentPlayersAction, Is.EqualTo(Faction.rebellion));
            That(GetPlayer().Opponent?.Hand, Has.Count.EqualTo(5));

            AssertForceIncreasedBy(Faction.rebellion, 1);
        }
    }
}