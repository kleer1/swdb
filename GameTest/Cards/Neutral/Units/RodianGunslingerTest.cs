using Game.Cards.Common.Models.Interface;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Rebellion.Units;
using SWDB.Game.Common;

namespace GameTest.Cards.Neutral.Units
{
    [TestFixture]
    public class RodianGunslingerTest : NeutralIPlayableCardTest
    {
        public override int Id => 94;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire, 2, 0);
            AssertGameState(Game.Rebel, 0, 0);
            AssertNoForceChange();

            IPlayableCard uwing = MoveToGalaxyRow(typeof(UWing)).ElementAt(0);

            Game.ApplyAction(Action.AttackCenterRow, uwing.Id);
            AssertGameState(Game.Empire, 4, 0);
            AssertGameState(Game.Rebel, 0, 0);
            AssertNoForceChange();

            Game.ApplyAction(Action.SelectAttacker, Card.Id);
            Game.ApplyAction(Action.ConfirmAttackers);

            That(uwing.Location, Is.EqualTo(CardLocation.GalaxyDiscard));

            Game.ApplyAction(Action.PassTurn);
            Game.ApplyAction(Action.PassTurn);

            Card.MoveToInPlay();

            AssertGameState(Game.Empire,2, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();

            Game.ApplyAction(Action.AttackBase, GetPlayer().Opponent?.CurrentBase?.Id);
            AssertGameState(Game.Empire,2, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }
    }
}