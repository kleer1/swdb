using Game.Cards.Common.Models.Interface;
using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Units
{
    [TestFixture]
    public class TieBomberTest : EmpireTargetableCardTest, IHasAbilityCardTest
    {
        private int rowCard = -1;

        public override int Id => 16;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,2, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ExileCard));

            IPlayableCard card1 = (IPlayableCard) Game.CardMap[40];
            card1.MoveToDiscard();

            Game.ApplyAction(Action.ExileCard, 40);
            That(card1.Location, Is.EqualTo(CardLocation.Exile));
            IsNull(card1.Owner);
            That(card1.CardList, Is.EqualTo(Game.ExiledCards));
            That(Game.PendingActions, Has.Count.EqualTo(0));
        }

        public void SetupAbility() 
        {
            rowCard = Game.GalaxyRow.ElementAt(0).Id;
        }

        public void VerifyAbility()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.DiscardCardFromCenter));
            That(Game.GalaxyRow, Has.Count.EqualTo(6));
            Game.ApplyAction(Action.DiscardCardFromCenter, rowCard);
            IPlayableCard card1 = (IPlayableCard) Game.CardMap[rowCard];
            That(card1.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(Game.GalaxyRow, Has.Count.EqualTo(6));
        }
    }
}