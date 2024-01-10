using GameTest.Cards.Bases.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Empire.Units;
using SWDB.Game.Cards.Rebellion.Ships;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Bases
{
    [TestFixture]
    public class HothTest : RebelAvailableBaseTest, IHasAtStartOfTurnTest
    {
        public override int Id => 131;

        public void AssertAfterStartOfTurn()
        {
            Player player = GetPlayer();
            PlayableCard rebelTransport = MoveToInPlay(typeof(RebelTransport), player).ElementAt(0);
            Game.ApplyAction(Action.PassTurn);

            PlayableCard deathTrooper = MoveToInPlay(typeof(DeathTrooper), player.Opponent).ElementAt(0);
            Game.ApplyAction(SWDB.Game.Actions.Action.AttackBase, Base.Id);
            Game.ApplyAction(SWDB.Game.Actions.Action.SelectAttacker, deathTrooper.Id);
            Game.ApplyAction(Action.ConfirmAttackers);

            That(rebelTransport.Location, Is.EqualTo(CardLocation.RebelDiscard));
            That(Base.CurrentDamage, Is.EqualTo(0));

            PlayableCard intercept = MoveToInPlay(typeof(TieInterceptor), GetPlayer().Opponent).ElementAt(0);
            Game.ApplyAction(SWDB.Game.Actions.Action.AttackBase, Base.Id);
            Game.ApplyAction(SWDB.Game.Actions.Action.SelectAttacker, intercept.Id);
            Game.ApplyAction(Action.ConfirmAttackers);

            That(Base.CurrentDamage, Is.EqualTo(2));

            Game.ApplyAction(Action.PassTurn);
            Game.ApplyAction(Action.PassTurn);

            deathTrooper.MoveToInPlay();
            intercept.MoveToInPlay();

            Game.ApplyAction(SWDB.Game.Actions.Action.AttackBase, Base.Id);
            Game.ApplyAction(SWDB.Game.Actions.Action.SelectAttacker, deathTrooper.Id);
            Game.ApplyAction(Action.ConfirmAttackers);

            That(Base.CurrentDamage, Is.EqualTo(3));

            Game.ApplyAction(SWDB.Game.Actions.Action.AttackBase, Base.Id);
            Game.ApplyAction(SWDB.Game.Actions.Action.SelectAttacker, intercept.Id);
            Game.ApplyAction(Action.ConfirmAttackers);

            That(Base.CurrentDamage, Is.EqualTo(6));
        }
    }
}