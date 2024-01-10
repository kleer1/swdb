using GameTest.Cards.Bases.Interfaces;

namespace GameTest.Cards.Empire.Bases
{
    [TestFixture]
    public class MustafarTest : EmpireAvailableBaseTest, IHasOnRevealTest
    {
        public override int Id => 122;

        public void PreChooseBaseSetup() 
        {
            GetPlayer().Opponent?.AddForce(1);
            That(Game.ForceBalance.Position, Is.EqualTo(4));
        }

        
        public void AssertAfterChooseBase() 
        {
            That(Game.ForceBalance.Position, Is.EqualTo(0));
        }
    }
}