using Game.Cards.Common.Models.Interface;
using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Units
{
    [TestFixture]
    public class SnowspeederTest : RebelTargetableCardTest, IHasAbilityCardTest
    {
        public override int Id => 52;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,2, 0);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ExileCard));

            IPlayableCard? card1 = GetPlayer().Opponent?.Hand.BaseList.ElementAt(0);
            card1?.MoveToDiscard();

            Game.ApplyAction(Action.ExileCard, card1?.Id);
            That(card1?.Location, Is.EqualTo(CardLocation.Exile));
            That(card1?.Owner, Is.Null);
            That(card1?.CardList, Is.EqualTo(Game.ExiledCards));
            That(Game.PendingActions, Has.Count.EqualTo(0));
        }

        public void VerifyAbility()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.DiscardFromHand));
            That(Game.CurrentPlayersAction, Is.EqualTo(Faction.empire));

            IPlayableCard? card1 = GetPlayer().Opponent?.Hand.BaseList.ElementAt(0);
            Game.ApplyAction(Action.DiscardFromHand, card1?.Id);

            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(Game.CurrentPlayersAction, Is.EqualTo(Faction.rebellion));
            That(card1?.Location, Is.EqualTo(CardLocationHelper.GetDiscard(GetPlayer().Opponent?.Faction ?? Faction.neutral)));
        }
    }
}