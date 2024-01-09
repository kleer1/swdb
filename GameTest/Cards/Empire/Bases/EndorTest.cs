using GameTest.Cards.Bases.Interfaces;
using SWDB.Game;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Empire.Units;
using SWDB.Game.Cards.Empire.Units.Starter;

namespace GameTest.Cards.Empire.Bases
{
    [TestFixture]
    public class EndorTest : EmpireAvailableBaseTest, IHasOnRevealTest, IHasAtStartOfTurnTest
    {
        public override int Id => 128;

        public void AssertAfterChooseBase() 
        {
            That(Game.StaticEffects, Has.Count.EqualTo(1));
            That(Game.StaticEffects.ElementAt(0), Is.EqualTo(StaticEffect.EndorBonus));

            PlayableCard stormtrooper = MoveToInPlay(typeof(Stormtrooper), GetPlayer()).ElementAt(0);
            That(GetPlayer().GetAvailableAttack(), Is.EqualTo(3));
            stormtrooper.MoveToDiscard();

            MoveToInPlay(typeof(AtAt), GetPlayer());
            That(GetPlayer().GetAvailableAttack(), Is.EqualTo(7));
            stormtrooper.MoveToInPlay();
            That(GetPlayer().GetAvailableAttack(), Is.EqualTo(10));
            MoveToInPlay(typeof(BobaFett), GetPlayer());
            That(GetPlayer().GetAvailableAttack(), Is.EqualTo(15));
        }

        public void AssertAfterStartOfTurn() {
            That(Game.StaticEffects, Has.Count.EqualTo(1));
            That(Game.StaticEffects.ElementAt(0), Is.EqualTo(StaticEffect.EndorBonus));
        }
    }
}