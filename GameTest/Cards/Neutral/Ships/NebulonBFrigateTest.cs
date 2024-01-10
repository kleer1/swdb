using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Common;

namespace GameTest.Cards.Neutral.Ships
{
    [TestFixture]
    public class NebulonBFrigateTest : NeutralIPlayableCardTest, IHasAbilityCardTest
    {
        public override int Id => 118;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public void SetupAbility() 
        {
            GetPlayer().CurrentBase?.AddDamage(4);
            That(GetPlayer().CurrentBase?.CurrentDamage, Is.EqualTo(4));
        }

        public void VerifyAbility()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ChooseResourceOrRepair));
            Game.ApplyAction(Action.ChooseResourceOrRepair, resourceOrRepair: ResourceOrRepair.Repair);
            That(GetPlayer().CurrentBase?.CurrentDamage, Is.EqualTo(1));
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
            That(GetPlayer().CurrentBase?.CurrentDamage, Is.EqualTo(4));
            AssertGameState(GetPlayer(), 0, 3);
            AssertNoForceChange();
        }

        [Test]
        public void TestActionWithoutDamagedBase() 
        {
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);
            AssertGameState(GetPlayer(), 0, 3);
        }
    }
}