using GameTest.Cards.PlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Neutral.Ships
{
    [TestFixture]
    public class CrocCruiserTest : NeutralPlayableCardTest, IHasAbilityCardTest
    {
        public override int Id => 113;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 1);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public void SetupAbility() 
        {
            GetPlayer().CurrentBase?.AddDamage(4);
        }

        public void VerifyAbility()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.DiscardFromHand));
            That(GetPlayer().CurrentBase?.CurrentDamage, Is.EqualTo(4));

            PlayableCard card1 = GetPlayer().Hand.BaseList.ElementAt(0);
            Game.ApplyAction(Action.DiscardFromHand, card1.Id);

            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(GetPlayer().CurrentBase?.CurrentDamage, Is.EqualTo(1));
            That(card1.Location, Is.EqualTo(CardLocation.EmpireDiscard));
        }
    }
}