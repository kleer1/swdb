using SWDB.Game.Cards.Empire.Ships;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Units
{
    [TestFixture]
    public class AdmiralPiettTest : EmpireTargetableCardTest
    {
        public override int Id => 26;

        public override void AssertAfterPlay()
        {
            AssertGameState(Game.Empire, 10, 2);
            AssertGameState(Game.Rebel, 0, 0);
            AssertNoForceChange();

            Card.MoveToDiscard();
            That(Game.Empire.GetAvailableAttack(), Is.EqualTo(8));
        }

        public override void AssertReward()
        {
            AssertForceIncreasedBy(Faction.rebellion, 1);
        }

         protected override void PrePlaySetup() 
         {
            // get two star destroyers in play
            MoveToInPlay(typeof(StarDestroyer), Game.Empire, 2);

            That(Game.Empire.GetAvailableAttack(), Is.EqualTo(8));
        }
    }
}