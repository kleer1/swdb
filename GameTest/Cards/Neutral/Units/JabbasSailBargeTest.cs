using Game.Cards.Common.Models.Interface;
using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Empire.Units;
using SWDB.Game.Cards.Empire.Units.Starter;
using SWDB.Game.Common;

namespace GameTest.Cards.Neutral.Units
{
    [TestFixture]
    public class JabbasSailBargeTest : NeutralIPlayableCardTest, IHasAbilityCardTest
    {
        private IPlayableCard? bounty;
        private IPlayableCard? nonBounty;

        public override int Id => 111;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,4, 3);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public void SetupAbility() 
        {
            bounty = MoveToInPlay(typeof(BobaFett), GetPlayer()).ElementAt(0);
            bounty.MoveToDiscard();
            nonBounty = MoveToInPlay(typeof(Stormtrooper), GetPlayer()).ElementAt(0);
            nonBounty.MoveToDiscard();
        }

        public void VerifyAbility()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ReturnCardToHand));

            Game.ApplyAction(Action.ReturnCardToHand, bounty?.Id);

            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(bounty?.Location, Is.EqualTo(CardLocation.EmpireHand));
            That(nonBounty?.Location, Is.EqualTo(CardLocation.EmpireDiscard));
        }

        [Test]
        public void TestSelectNonBounty() 
        {
            SetupAbility();
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);

            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ReturnCardToHand));

            Game.ApplyAction(Action.ReturnCardToHand, nonBounty?.Id);

            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ReturnCardToHand));
            That(bounty?.Location, Is.EqualTo(CardLocation.EmpireDiscard));
            That(nonBounty?.Location, Is.EqualTo(CardLocation.EmpireDiscard));
        }
    }
}