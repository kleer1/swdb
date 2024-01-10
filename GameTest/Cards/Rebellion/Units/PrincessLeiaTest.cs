using GameTest.Cards.PlayableCards.Interfaces;
using SWDB.Game;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Rebellion.Units;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Units
{
    [TestFixture]
    public class PrincessLeiaTest : RebelTargetableCardTest, IHasAbilityCardTest
    {
        private PlayableCard? luke;

        public override int Id => 71;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,2, 2);
            AssertForceIncreasedBy(Faction.rebellion, 2);
        }

        public override void AssertReward()
        {
            AssertGameState(Game.Empire,0, 3);
            AssertGameState(Game.Rebel,0, 0);
            AssertForceIncreasedBy(Faction.empire, 3);
        }

        public void SetupAbility() 
        {
            luke = MoveToGalaxyRow(typeof(LukeSkywalker)).ElementAt(0);
        }

        public void VerifyAbility()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.PurchaseCard));
            That(Game.StaticEffects, Has.Count.EqualTo(2));
            That(Game.StaticEffects, Is.EquivalentTo(new List<StaticEffect> {
                StaticEffect.NextFactionPurchaseIsFree, StaticEffect.BuyNextToTopOfDeck
            }));

            Game.ApplyAction(Action.PurchaseCard, luke?.Id);

            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(Game.StaticEffects, Has.Count.EqualTo(0));
            That(luke?.Location, Is.EqualTo(CardLocation.RebelDeck));
            That(GetPlayer().Deck.ElementAt(0), Is.EqualTo(luke));
        }

        [Test]
        public void TestForceNotWithYou() 
        {
            luke = MoveToGalaxyRow(typeof(LukeSkywalker)).ElementAt(0);
            GetPlayer().Opponent?.AddForce(100);
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);

            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.PurchaseCard));
            That(Game.StaticEffects, Has.Count.EqualTo(1));
            That(Game.StaticEffects.ElementAt(0), Is.EqualTo(StaticEffect.NextFactionPurchaseIsFree));

            Game.ApplyAction(Action.PurchaseCard, luke?.Id);

            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(Game.StaticEffects, Has.Count.EqualTo(0));
            That(luke?.Location, Is.EqualTo(CardLocation.RebelDiscard));
        }
    }
}