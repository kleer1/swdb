using GameTest.Cards.Bases.Interfaces;
using GameTest.Cards.Empire.Bases;
using SWDB.Game;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.BasesThat
{
    [TestFixture]
    public class KafreneTest : EmpireAvailableBaseTest, IHasOnRevealTest, IHasAtStartOfTurnTest
    {
        public override int Id => 125;

        public void AssertAfterStartOfTurn() 
        {
            That(Game.StaticEffects, Has.Count.EqualTo(1));
            That(Game.StaticEffects.ElementAt(0), Is.EqualTo(StaticEffect.DrawOnFirstNeutralCard));
        }

        public void AssertAfterChooseBase() 
        {
            That(Game.StaticEffects, Has.Count.EqualTo(1));
            That(Game.StaticEffects.ElementAt(0), Is.EqualTo(StaticEffect.DrawOnFirstNeutralCard));

            // play a neutral card
            PlayableCard neutral = (PlayableCard) Game.CardMap[BaseTest.NEUTRAL_GALAXY_CARD];
            neutral.BuyToHand(GetPlayer());
            GetPlayer().DrawCards(1);
            That(GetPlayer().Hand, Has.Count.EqualTo(7));

            PlayableCard topCard = GetPlayer().Deck.BaseList.ElementAt(0);

            Game.ApplyAction(Action.PlayCard, neutral.Id);
            That(GetPlayer().Hand, Has.Count.EqualTo(7));
            That(topCard.Location, Is.EqualTo(CardLocationHelper.GetHand(GetPlayer().Faction)));
            That(neutral.Location, Is.EqualTo(CardLocationHelper.GetUnitsInPlay(GetPlayer().Faction)));
            That(Game.StaticEffects, Has.Count.EqualTo(0));
        }
    }
}