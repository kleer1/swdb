using GameTest.Cards.Bases.Interfaces;
using SWDB.Game;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Empire.Units.Starter;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Bases
{
    public class OrdMantellTest : EmpireAvailableBaseTest, IHasOnRevealTest, IHasAtStartOfTurnTest
    {
        public override int Id => 127;
        public void AssertAfterChooseBase() 
        {
            That(Game.StaticEffects, Has.Count.EqualTo(1));
            That(Game.StaticEffects.ElementAt(0), Is.EqualTo(StaticEffect.CanBountyOneNeutral));

            // Confirm we can attack a neutral card
            PlayableCard neutral = (PlayableCard) Game.CardMap[NEUTRAL_GALAXY_CARD];
            neutral.MoveToGalaxyRow();

            PlayableCard stormtrooper = MoveToInPlay(typeof(Stormtrooper), GetPlayer()).ElementAt(0);

            Game.ApplyAction(Action.AttackNeutralCard);
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.AttackCenterRow));

            Game.ApplyAction(Action.AttackCenterRow, neutral.Id);
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.SelectAttacker));

            Game.ApplyAction(Action.SelectAttacker, stormtrooper.Id);
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.SelectAttacker));

            Game.ApplyAction(Action.ConfirmAttackers);
            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(Game.StaticEffects, Has.Count.EqualTo(0));
            That(neutral.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(GetPlayer().Resources, Is.EqualTo(neutral.Cost));
        }

        public void AssertAfterStartOfTurn() 
        {
            That(Game.StaticEffects, Has.Count.EqualTo(1));
            That(Game.StaticEffects.ElementAt(0), Is.EqualTo(StaticEffect.CanBountyOneNeutral));
        }
    }
}