using Game.Cards.Common.Models.Interface;
using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Units 
{
    [TestFixture]
    public class AtAtTest : EmpireTargetableCardTest, IHasAbilityCardTest
    {
        private static readonly int TROOPER_ID = 7;

        public override int Id => 25;

        public void SetupAbility() 
        {
        IPlayableCard stormtooper = (IPlayableCard) Game.CardMap[TROOPER_ID];
            stormtooper.MoveToDiscard();
        }

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,6, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.PurchaseCard));
            That(Game.StaticEffects, Has.Count.EqualTo(1));
            That(Game.StaticEffects.ElementAt(0), Is.EqualTo(StaticEffect.NextFactionPurchaseIsFree));

            // check that I can buy for free
            That(Game.Rebel.Resources, Is.EqualTo(0));

            // Move Luke to buy row
            IPlayableCard luke = (IPlayableCard) Game.CardMap[LUKE_ID];
            luke.MoveToGalaxyRow();

            // Try to buy
            Game.ApplyAction(Action.PurchaseCard, LUKE_ID);

            That(luke.Location, Is.EqualTo(CardLocation.RebelDiscard));
            That(luke.Owner, Is.EqualTo(Game.Rebel));
            That(luke.CardList, Is.EqualTo(Game.Rebel.Discard));
            That(Game.Rebel.Resources, Is.EqualTo(0));
        }

        public void VerifyAbility()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ReturnCardToHand));
            Game.ApplyAction(Action.ReturnCardToHand, TROOPER_ID);
            IPlayableCard stormtooper = (IPlayableCard) Game.CardMap[TROOPER_ID];
            That(stormtooper.Location, Is.EqualTo(CardLocation.EmpireHand));
        }
    }
}