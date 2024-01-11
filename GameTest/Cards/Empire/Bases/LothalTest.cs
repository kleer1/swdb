using Game.Common.Interfaces;
using GameTest.Cards.Bases;

namespace GameTest.Cards.Empire.Bases
{
    [TestFixture]
    public class LothalTest : StartingBaseTest
    {
        public override IPlayer GetPlayer()
        {
            return Game.Empire;
        }

        public override int Id => 120;
    }
}