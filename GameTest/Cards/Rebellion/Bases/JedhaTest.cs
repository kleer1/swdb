using Game.Cards.Common.Models.Interface;
using GameTest.Cards.Bases.Interfaces;
using SWDB.Game;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Neutral.Units;
using SWDB.Game.Cards.Rebellion.Units;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Bases
{
    [TestFixture]
    public class JedhaTest : RebelAvailableBaseTest, IHasOnRevealTest, IHasAtStartOfTurnTest
    {
        public override int Id => 136;

        public void AssertAfterChooseBase()
        {
            That(Game.StaticEffects, Has.Count.EqualTo(2));
            That(Game.StaticEffects, Is.EquivalentTo(new List<StaticEffect> {
                StaticEffect.ExileNextNeutralPurchase,
                StaticEffect.BuyNextNeutralToHand
            }));

            IPlayableCard ywing = MoveToGalaxyRow(typeof(YWing)).ElementAt(0);

            GetPlayer().AddResources(1);

            Game.ApplyAction(Action.PurchaseCard, ywing.Id);

            That(GetPlayer().Resources, Is.EqualTo(0));
            That(ywing.Location, Is.EqualTo(CardLocation.RebelDiscard));
            That(Game.StaticEffects, Has.Count.EqualTo(2));
            That(Game.StaticEffects, Is.EquivalentTo( new List<StaticEffect> {
                StaticEffect.ExileNextNeutralPurchase,
                StaticEffect.BuyNextNeutralToHand
            }));

            IPlayableCard z95 = MoveToGalaxyRow(typeof(Z95Headhunter)).ElementAt(0);
            GetPlayer().AddResources(1);

            Game.ApplyAction(Action.PurchaseCard, z95.Id);

            That(GetPlayer().Resources, Is.EqualTo(0));
            That(Game.StaticEffects, Has.Count.EqualTo(0));
            That(z95.Location, Is.EqualTo(CardLocation.RebelHand));

            Game.ApplyAction(Action.PassTurn);

            That(z95.Location, Is.EqualTo(CardLocation.Exile));
        }

        public void AssertAfterStartOfTurn()
        {
            That(Game.StaticEffects, Has.Count.EqualTo(2));
            That(Game.StaticEffects, Is.EquivalentTo( new List<StaticEffect> {
                StaticEffect.ExileNextNeutralPurchase,
                StaticEffect.BuyNextNeutralToHand
            }));

            IPlayableCard z95 = MoveToGalaxyRow(typeof(Z95Headhunter)).ElementAt(0);
            GetPlayer().AddResources(1);

            Game.ApplyAction(Action.PurchaseCard, z95.Id);

            That(GetPlayer().Resources, Is.EqualTo(0));
            That(Game.StaticEffects, Has.Count.EqualTo(0));
            That(z95.Location, Is.EqualTo(CardLocation.RebelHand));

            Game.ApplyAction(Action.PassTurn);

            That(z95.Location, Is.EqualTo(CardLocation.Exile));
        }
    }
}