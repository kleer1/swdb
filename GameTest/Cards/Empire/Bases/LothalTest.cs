using GameTest.Cards.Bases;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Bases
{
    [TestFixture]
    public class LothalTest : StartingBaseTest
    {
        public override Player GetPlayer()
        {
            return Game.Empire;
        }

        public override int Id => 120;
    }
}