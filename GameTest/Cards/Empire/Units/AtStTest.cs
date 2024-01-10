
using Game.Cards.Common.Models.Interface;
using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Units
{
    [TestFixture]
    public class AtStTest : EmpireTargetableCardTest, IHasAbilityCardTest
    {
        private static readonly int REBEL_CARD_ID_1 = 40;
        private static readonly int REBEL_CARD_Id_2 = 41;

        public override int Id => 22;

        public void SetupAbility() 
        {
            IPlayableCard luke = (IPlayableCard) Game.CardMap[LUKE_ID];
            luke.MoveToGalaxyRow();
        }

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,4, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ExileCard));

            IPlayableCard card1 = (IPlayableCard) Game.CardMap[REBEL_CARD_ID_1];
            card1.MoveToDiscard();
            IPlayableCard card2 = (IPlayableCard) Game.CardMap[REBEL_CARD_Id_2];
            card2.MoveToHand();

            Game.ApplyAction(Action.ExileCard, REBEL_CARD_ID_1);
            That(card1.Location, Is.EqualTo(CardLocation.Exile));
            That(card1.Owner, Is.Null);
            That(card1.CardList, Is.EqualTo(Game.ExiledCards));
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.ExileCard));

            Game.ApplyAction(Action.ExileCard, REBEL_CARD_Id_2);
            That(card2.Location, Is.EqualTo(CardLocation.Exile));
            That(card2.Owner, Is.Null);
            That(card2.CardList, Is.EqualTo(Game.ExiledCards));
            That(Game.PendingActions, Has.Count.EqualTo(0));
        }

        public void VerifyAbility()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.DiscardCardFromCenter));
            Game.ApplyAction(Action.DiscardCardFromCenter, LUKE_ID);
            IPlayableCard luke = (IPlayableCard) Game.CardMap[LUKE_ID];
            That(luke.Location, Is.EqualTo(CardLocation.GalaxyDiscard));
            That(luke.Owner, Is.Null);
            That(luke.CardList, Is.EqualTo(Game.GalaxyDiscard));
        }
    }
}