using SWDB.Game.Actions;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Units.Starter
{
    [TestFixture]
    public class InquisitorTest : EmpirePlayableCardTest
    {
        public override int Id => 9;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();

            That(Game.PendingActions, Has.Count.EqualTo(1));
            PendingAction pendingAction = Game.PendingActions.ElementAt(0);
            That(pendingAction.Action, Is.EqualTo(Action.ChooseStatBoost));
        }

        [Test]
        public void TestChooseAttack() 
        {
            Game.ApplyAction(Action.PlayCard, Id);
            Game.ApplyAction(Action.ChooseStatBoost, stats: Stats.Attack);
            AssertGameState(Game.Empire, 1, 0);
            AssertGameState(Game.Rebel, 0, 0);
            AssertNoForceChange();
            That(Game.PendingActions, Has.Count.EqualTo(0));
        }

        [Test]
        public void TestChooseResource() 
        {
            Game.ApplyAction(Action.PlayCard, Id);
            Game.ApplyAction(Action.ChooseStatBoost, stats: Stats.Resources);
            AssertGameState(Game.Empire,0, 1);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
            That(Game.PendingActions, Has.Count.EqualTo(0));
        }

        [Test]
        public void TestChooseForce() 
        {
            Game.ApplyAction(Action.PlayCard, Id);
            Game.ApplyAction(Action.ChooseStatBoost, stats: Stats.Force);
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertForceIncreasedBy(Faction.empire, 1);
            That(Game.PendingActions, Has.Count.EqualTo(0));
        }
    }
}