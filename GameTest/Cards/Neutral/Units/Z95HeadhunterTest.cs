using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Cards.Empire.Ships;
using SWDB.Game.Common;

namespace GameTest.Cards.Neutral.Units
{
    [TestFixture]
    public class Z95HeadhunterTest : NeutralIPlayableCardTest, IHasAbilityCardTest
    {
        public override int Id => 90;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,2, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public void SetupAbility() 
        {
            Player player = GetPlayer();
            if (player.Opponent == null) return;
            MoveToInPlay(typeof(StarDestroyer), player.Opponent);
        }

        public void VerifyAbility()
        {
            That(GetPlayer().Hand, Has.Count.EqualTo(6));
        }

        [Test]
        public void TestNoCapitalShip() 
        {
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);
            That(GetPlayer().Hand, Has.Count.EqualTo(5));
        }
    }
}