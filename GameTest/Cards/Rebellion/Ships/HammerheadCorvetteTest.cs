using Game.Cards.Common.Models.Interface;
using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Empire.Ships;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Ships
{
    [TestFixture]
    public class HammerheadCorvetteTest : RebelIPlayableCardTest, IHasAbilityCardTest
    {
        private IPlayableCard? empShip;

        public override int Id => 76;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,0, 2);
            AssertNoForceChange();

            Game.ApplyAction(Action.PassTurn);

            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();

            Game.ApplyAction(Action.PassTurn);

            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,0, 2);
            AssertNoForceChange();
        }

        public void SetupAbility() 
        {
            empShip = MoveToInPlay(typeof(StarDestroyer), GetPlayer().Opponent).ElementAt(0);
        }

        public void VerifyAbility()
        {
            That(Card.Location, Is.EqualTo(CardLocation.Exile));
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.HammerHeadAway));

            Game.ApplyAction(Action.HammerHeadAway, empShip?.Id);

            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(empShip?.Location, Is.EqualTo(CardLocationHelper.GetDiscard(GetPlayer().Opponent?.Faction ?? Faction.neutral)));
        }

        [Test]
        public void TestDestroyFromGalaxyRow() 
        {
            empShip = MoveToGalaxyRow(typeof(StarDestroyer)).ElementAt(0);

            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);

            That(Card.Location, Is.EqualTo(CardLocation.Exile));
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.HammerHeadAway));

            Game.ApplyAction(Action.HammerHeadAway, empShip.Id);

            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(empShip.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
        }
    }
}