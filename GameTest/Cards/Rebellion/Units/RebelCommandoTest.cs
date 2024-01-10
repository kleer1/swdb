using GameTest.Cards.PlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Units
{
    [TestFixture]
    public class RebelCommandoTest : RebelTargetableCardTest, IHasAbilityCardTest
    {
        public override int Id => 56;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,3, 0);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            AssertForceIncreasedBy(Faction.empire, 2);
        }

        public void SetupAbility() 
        {
            GetPlayer().AddForce(1);
        }

        public void VerifyAbility()
        {
            That(Game.Empire.Hand, Has.Count.EqualTo(4));
        }

        [Test]
        public void TestForceNotWithYou() 
        {
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);

            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.DiscardFromHand));
            That(Game.CurrentPlayersAction, Is.EqualTo(Faction.empire));

            PlayableCard? card1 = GetPlayer().Opponent?.Hand.BaseList.ElementAt(0);
            Game.ApplyAction(Action.DiscardFromHand, card1?.Id);

            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(Game.CurrentPlayersAction, Is.EqualTo(Faction.rebellion));
            That(card1?.Location, Is.EqualTo(CardLocationHelper.GetDiscard(GetPlayer().Opponent?.Faction ?? Faction.neutral)));
        }
    }
}