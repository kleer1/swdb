using GameTest.Cards.Bases.Interfaces;
using SWDB.Game;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Rebellion.Units;

namespace GameTest.Cards.Rebellion.Bases
{
    [TestFixture]
    public class Yavin4Test : RebelAvailableBaseTest, IHasAtStartOfTurnTest, IHasOnRevealTest
    {
        public override int Id => 138;

        public void AssertAfterChooseBase()
        {
            That(Game.StaticEffects, Has.Count.EqualTo(1));
            That(Game.StaticEffects.ElementAt(0), Is.EqualTo(StaticEffect.Yavin4Effect));
        }

        public void AssertAfterStartOfTurn()
        {
            That(Game.StaticEffects, Has.Count.EqualTo(1));
            That(Game.StaticEffects.ElementAt(0), Is.EqualTo(StaticEffect.Yavin4Effect));

            That(GetPlayer().Opponent?.CurrentBase?.CurrentDamage, Is.EqualTo(0));

            // set up rebel commando random discard
            GetPlayer().AddForce(1);
            PlayableCard rc = MoveToInPlay(typeof(RebelCommando), GetPlayer()).ElementAt(0);
            Game.ApplyAction(Action.UseCardAbility, rc.Id);

            That(GetPlayer().Opponent?.Hand, Has.Count.EqualTo(4));
            That(GetPlayer().Opponent?.CurrentBase?.CurrentDamage, Is.EqualTo(2));

            // do it again with snow speeder to test both paths
            PlayableCard snow = MoveToInPlay(typeof(Snowspeeder), GetPlayer()).ElementAt(0);
            Game.ApplyAction(Action.UseCardAbility, snow.Id);

            PlayableCard? card1 = GetPlayer().Opponent?.Hand.BaseList.ElementAt(0);
            Game.ApplyAction(Action.DiscardFromHand, card1?.Id);

            That(GetPlayer().Opponent?.Hand, Has.Count.EqualTo(3));
            That(GetPlayer().Opponent?.CurrentBase?.CurrentDamage, Is.EqualTo(4));
        }
    }
}