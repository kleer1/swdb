using Game.Cards.Common.Models.Interface;
using GameTest.Cards.Interfaces;
using SWDB.Game;
using SWDB.Game.Cards.Common.Models;

namespace GameTest.Cards.IPlayableCards.Interfaces
{
    public interface IHasAbilityCardTest : IBaseIPlayableCard
    {
        [Test]
        void TestUseCardAbility()
        {
            SWDBGame game = Game;
            IPlayableCard card = Card;
            SetupAbility();
            UseCardAbility(game, card);
            VerifyAbility();
        }
        
        void UseCardAbility(SWDBGame game, IPlayableCard card) 
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