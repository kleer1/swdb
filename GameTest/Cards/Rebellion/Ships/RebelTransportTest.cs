using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Ships
{
    [TestFixture]
    public class RebelTransportTest : RebelIPlayableCardTest, IHasAbilityCardTest
    {
        public override int Id => 74;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public void SetupAbility() 
        {
            GetPlayer().CurrentBase?.AddDamage(3);
            That(GetPlayer().CurrentBase?.CurrentDamage, Is.EqualTo(3));
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
            That(GetPlayer().CurrentBase?.CurrentDamage, Is.EqualTo(3));
            AssertGameState(GetPlayer(), 0, 1);
            AssertNoForceChange();
        }

        [Test]
        public void TestActionWithoutDamagedBase() 
        {
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);
            AssertGameState(GetPlayer(), 0, 1);
        }
    }
}