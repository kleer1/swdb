using GameTest.Cards.Bases.Interfaces;
using SWDB.Game;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Rebellion.Units;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Bases
{
    [TestFixture]
    public class SullustTest : RebelAvailableBaseTest, IHasOnRevealTest, IHasAtStartOfTurnTest
    {
        public override int Id => 134;

        public void AssertAfterChooseBase()
        {
            That(Game.StaticEffects, Has.Count.EqualTo(1));
            That(Game.StaticEffects, Does.Contain(StaticEffect.BuyNextToTopOfDeck));
        }

        public void AssertAfterStartOfTurn()
        {
            That(Game.StaticEffects, Has.Count.EqualTo(1));
            That(Game.StaticEffects, Does.Contain(StaticEffect.BuyNextToTopOfDeck));

            PlayableCard luke = MoveToGalaxyRow(typeof(LukeSkywalker)).ElementAt(0);
            GetPlayer().AddResources(9);

            Game.ApplyAction(Action.PurchaseCard, luke.Id);

            That(Game.StaticEffects, Has.Count.EqualTo(0));
            That(luke.Location, Is.EqualTo(CardLocation.RebelDeck));
            That(GetPlayer().Deck.ElementAt(0), Is.EqualTo(luke));
            That(GetPlayer().Resources, Is.EqualTo(1));
        }
    }
}