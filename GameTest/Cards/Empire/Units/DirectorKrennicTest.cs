using GameTest.Cards.IPlayableCards.Interfaces;
using SWDB.Game.Cards.Common.Models;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Units
{
    [TestFixture]
    public class DirectorKrennicTest : EmpireTargetableCardTest, IHasAbilityCardTest
    {
        private static readonly int DEATH_STAR_Id = 121;

        public override int Id => 30;

        public void SetupAbility() 
        {
            GetPlayer().CurrentBase =null;
            Base ds = (Base) Game.CardMap[DEATH_STAR_Id];
            ds.MakeCurrentBase();
        }
    
        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire,3, 2);
            AssertGameState(Game.Rebel,0, 0);
            AssertNoForceChange();
        }

        public override void AssertReward()
        {
            That(Game.Rebel.Resources, Is.EqualTo(3));
            AssertForceIncreasedBy(Faction.rebellion, 2);
        }

        public void VerifyAbility()
        {
            That(GetPlayer().Hand, Has.Count.EqualTo(7));
        }

        [Test]
        public void TestAbilityWithoutDeathStar() 
        {
            ((IHasAbilityCardTest) this).UseCardAbility(Game, Card);
            That(GetPlayer().Hand, Has.Count.EqualTo(6));
        }
    }
}