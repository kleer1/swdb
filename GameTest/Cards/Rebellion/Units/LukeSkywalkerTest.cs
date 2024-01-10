using GameTest.Cards.PlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Cards.Empire.Ships;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Units
{
    [TestFixture]
    public class LukeSkywalkerTest : RebelTargetableCardTest, IHasAbilityCardTest
    {
        public override int Id => 73;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,6, 0);
            AssertForceIncreasedBy(Faction.rebellion, 2);
        }

        public void PreAttackSetup() 
        {
            GetPlayer().AddForce(1);
        }

        public override void AssertReward()
        {
            AssertGameState(Game.Empire,0, 4);
            AssertGameState(Game.Rebel,0, 0);
            AssertForceIncreasedBy(Faction.empire, 4);
        }

        public void SetupAbility() 
        {
            GetPlayer().AddForce(1);
            MoveToInPlay(typeof(StarDestroyer), GetPlayer().Opponent);
        }

        public void VerifyAbility()
        {
            That(Game.PendingActions, Has.Count.EqualTo(1));
            That(Game.PendingActions.ElementAt(0).Action, Is.EqualTo(Action.LukeDestroyShip));

            CapitalShip? card1 = GetPlayer().Opponent?.ShipsInPlay.BaseList.ElementAt(0);
            Game.ApplyAction(Action.LukeDestroyShip, card1?.Id);

            That(Game.PendingActions, Has.Count.EqualTo(0));
            That(card1?.Location, Is.EqualTo(CardLocationHelper.GetDiscard(GetPlayer().Opponent?.Faction ?? Faction.neutral)));
        }
    }
}