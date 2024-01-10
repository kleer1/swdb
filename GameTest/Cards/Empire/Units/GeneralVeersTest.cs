using GameTest.Cards.PlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Units
{
    [TestFixture]
    public class GeneralVeersTest : EmpireTargetableCardTest, IHasAbilityCardTest
    {
        private static readonly int VEHICLE_Id = 22;

        public override int Id => 27;

        public void SetupAbility() 
        {
            PlayableCard vehicle = (PlayableCard) Game.CardMap[VEHICLE_Id];
            vehicle.BuyToHand(GetPlayer());
            vehicle.MoveToInPlay();
        }

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,4, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
            }

        public override void AssertReward()
        {
            AssertForceIncreasedBy(Faction.rebellion, 3);
        }

        public void VerifyAbility()
        {
            That(GetPlayer().Hand, Has.Count.EqualTo(6));
        }
    }
}