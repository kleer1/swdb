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
            card.MoveToInPlay();
            game.ApplyAction(SWDB.Game.Actions.Action.UseCardAbility, card.Id);
            That(card.AbilityActive(), Is.False);
        }

        void SetupAbility() 
        {

        }

        void VerifyAbility();
    }
}