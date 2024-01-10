using GameTest.Cards.Bases.Interfaces;

namespace GameTest.Cards.Rebellion.Bases
{
    [TestFixture]
    public class AlderaanTest : RebelAvailableBaseTest, IHasOnRevealTest
    {
        public override int Id => 137;

        public void PreChooseBaseSetup() 
        {
            GetPlayer().Opponent?.AddForce(1);
            That(Game.ForceBalance.Position, Is.EqualTo(2));
        }

        public void AssertAfterChooseBase()
        {
            That(Game.ForceBalance.Position, Is.EqualTo(6));
        }
    }
}