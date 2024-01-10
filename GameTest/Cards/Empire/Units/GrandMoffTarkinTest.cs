using GameTest.Cards.PlayableCards.Interfaces;
using SWDB.Game;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Units
{
    [TestFixture]
    public class GrandMoffTarkinTest : EmpireTargetableCardTest, IHasAbilityCardTest
    {
        public override int Id => 31;

        public void SetupAbility() 
        {
            PlayableCard dv = (PlayableCard) Game.CardMap[32];
            dv.MoveToGalaxyRow();
        }

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,2, 2);
            AssertGameState(Game.Rebel,0, 0);
            AssertForceIncreasedBy(Faction.empire, 2);
        }

        public override void AssertReward()
        {
            AssertGameState(Game.Rebel, 0, 3);
            AssertForceIncreasedBy(Faction.rebellion, 3);
        }

        public void VerifyAbility()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.PurchaseCard));
            That(Game.StaticEffects, Has.Count.EqualTo(3));
            That(Game.StaticEffects, Is.EquivalentTo(new List<StaticEffect> { 
                StaticEffect.NextFactionPurchaseIsFree,
                StaticEffect.ExileNextFactionPurchase,
                StaticEffect.BuyNextToHand
            }));

            Game.ApplyAction(Action.PurchaseCard, 32);
            PlayableCard dv = (PlayableCard) Game.CardMap[32];
            That(dv.Location, Is.EqualTo(CardLocation.EmpireHand));

            Game.ApplyAction(Action.PassTurn);
            That(dv.Location, Is.EqualTo(CardLocation.Exile));
        }
    }
}