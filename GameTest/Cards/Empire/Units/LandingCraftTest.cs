using GameTest.Cards.PlayableCards.Interfaces;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Units
{
    [TestFixture]
    public class LandingCraftTest : EmpireTargetableCardTest, IHasAbilityCardTest
    {
        public override int Id => 23;

        public void SetupAbility() 
        {
            GetPlayer().CurrentBase?.AddDamage(1);
            That(GetPlayer().CurrentBase?.CurrentDamage, Is.EqualTo(1));
        }

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            AssertGameState(Game.Rebel,0, 4);
            AssertGameState(Game.Empire,0, 0);
            AssertNoForceChange();
        }

        public void VerifyAbility()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ChooseResourceOrRepair));
            Game.ApplyAction(Action.ChooseResourceOrRepair, resourceOrRepair: ResourceOrRepair.Repair);
            That(GetPlayer().CurrentBase?.CurrentDamage, Is.EqualTo(0));
            AssertNoForceChange();
        }

        [Test]
        public void TestChooseResourceWithDamagedBase() 
        {
            SetupAbility();
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ChooseResourceOrRepair));
            Game.ApplyAction(Action.ChooseResourceOrRepair, resourceOrRepair: ResourceOrRepair.Resources);
            That(GetPlayer().CurrentBase?.CurrentDamage, Is.EqualTo(1));
            AssertGameState(GetPlayer(), 0, 4);
            AssertNoForceChange();
        }

        [Test]
        public void TestActionWithoutDamagedBase() 
        {
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);
            AssertGameState(GetPlayer(), 0, 4);
        }
    }
}