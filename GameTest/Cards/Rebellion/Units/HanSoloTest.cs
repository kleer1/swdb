using GameTest.Cards.PlayableCards.Interfaces;
using SWDB.Game.Cards.Rebellion.Units;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Units
{
    [TestFixture]
    public class HanSoloTest : RebelTargetableCardTest, IHasAbilityCardTest
    {
        public override int Id => 69;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,3, 2);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            AssertGameState(Game.Empire, 0, 3);
            AssertForceIncreasedBy(Faction.empire, 2);
        }

        public void SetupAbility() 
        {
            MoveToInPlay(typeof(MillenniumFalcon), GetPlayer());
        }

        public void VerifyAbility()
        {
            That(GetPlayer().Hand, Has.Count.EqualTo(7));
        }

        [Test]
        public void TestNoMillFalcon() 
        {
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);
            That(GetPlayer().Hand, Has.Count.EqualTo(6));
        }
    }
}