using GameTest.Cards.PlayableCards.Interfaces;
using SWDB.Game;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Empire.Units;
using SWDB.Game.Cards.Rebellion.Units;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Units
{
    [TestFixture]
    public class MillenniumFalconTest : RebelTargetableCardTest, IHasAbilityCardTest
    {
        private PlayableCard? unique;

        public override int Id => 72;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,5, 2);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.PurchaseCard));
            That(Game.StaticEffects, Has.Count.EqualTo(1));
            That(Game.StaticEffects.ElementAt(0), Is.EqualTo(StaticEffect.NextFactionPurchaseIsFree));

            // check that I can buy for free
            That(Game.Empire.Resources, Is.EqualTo(0));

            PlayableCard darth = MoveToGalaxyRow(typeof(DarthVader)).ElementAt(0);

            // Try to buy
            Game.ApplyAction(Action.PurchaseCard, darth.Id);

            That(darth.Location, Is.EqualTo(CardLocation.EmpireDiscard));
            That(darth.Owner, Is.EqualTo(Game.Empire));
            That(darth.CardList, Is.EqualTo(Game.Empire.Discard));
            That(Game.Empire.Resources, Is.EqualTo(0));
        }

        public void SetupAbility() 
        {
            unique = MoveToInPlay(typeof(HanSolo), GetPlayer()).ElementAt(0);
            unique?.MoveToDiscard();
        }

        public void VerifyAbility()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ReturnCardToHand));

            Game.ApplyAction(Action.ReturnCardToHand, unique?.Id);
            That(unique?.Location, Is.EqualTo(CardLocation.RebelHand));
        }

        [Test]
        public void TestReturnNonUnique() 
        {
            unique = MoveToInPlay(typeof(HanSolo), GetPlayer()).ElementAt(0);
            unique?.MoveToDiscard();
            PlayableCard nonUnique = MoveToInPlay(typeof(BWing), GetPlayer()).ElementAt(0);
            nonUnique.MoveToDiscard();

            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ReturnCardToHand));

            Game.ApplyAction(Action.ReturnCardToHand, nonUnique.Id);
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ReturnCardToHand));
            That(nonUnique.Location, Is.EqualTo(CardLocation.RebelDiscard));
        }
    }
}