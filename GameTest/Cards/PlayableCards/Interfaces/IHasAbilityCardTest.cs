using GameTest.Cards.Interfaces;
using SWDB.Game;
using SWDB.Game.Cards.Common.Models;

namespace GameTest.Cards.PlayableCards.Interfaces
{
    public interface IHasAbilityCardTest : IBasePlayableCard
    {
        [Test]
        void TestUseCardAbility()
        {
            SWDBGame game = Game;
            PlayableCard card = Card;
            SetupAbility();
            UseCardAbility(game, card);
            VerifyAbility();
        }
        
        void UseCardAbility(SWDBGame game, PlayableCard card) 
        {
            Card.MoveToInPlay();
            Game.ApplyAction(SWDB.Game.Actions.Action.UseCardAbility, Card.Id);
            That(Card.AbilityActive(), Is.False);
        }

        void SetupAbility() 
        {

        }

        void VerifyAbility();
    }
}