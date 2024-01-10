using GameTest.Cards.PlayableCards.Interfaces;
using SWDB.Game.Cards.Rebellion.Units;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Units
{
    [TestFixture]
    public class ChewbaccaTest : RebelTargetableCardTest, IHasAbilityCardTest
    {
        public override int Id => 68;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,5, 0);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            AssertGameState(Game.Empire,0, 0);
            AssertGameState(Game.Rebel,0, 0);
            AssertForceIncreasedBy(Faction.empire, 3);
        }

        public void SetupAbility() 
        {
            MoveToInPlay(typeof(HanSolo), GetPlayer());
        }

        public void VerifyAbility()
        {
            That(GetPlayer().Hand, Has.Count.EqualTo(6));
        }

        [Test]
        public void TestNoUniqueInPlay() 
        {
            MoveToInPlay(typeof(BWing), GetPlayer());
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);
            That(GetPlayer().Hand, Has.Count.EqualTo(5));
        }
    }
}