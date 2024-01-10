using SWDB.Game.Actions;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Neutral.Units
{
    [TestFixture]
    public class LobotTest : NeutralPlayableCardTest
    {
        public override int Id => 106;

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
            AssertGameState(Game.Empire,2, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
            That(Game.PendingActions, Has.Count.EqualTo(0));
        }

        [Test]
        public void TestChooseResource() 
        {
            Game.ApplyAction(Action.PlayCard, Id);
            Game.ApplyAction(Action.ChooseStatBoost, stats: Stats.Resources);
            AssertGameState(Game.Empire,0, 2);
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
            AssertForceIncreasedBy(Faction.empire, 2);
            That(Game.PendingActions, Has.Count.EqualTo(0));
        }
    }
}