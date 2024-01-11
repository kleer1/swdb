using Game.Common.Interfaces;
using GameTest.Cards.Bases;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Bases
{
    [TestFixture]
    public class DantooineTest : StartingBaseTest
    {
        public override int Id => 130;

        public override IPlayer GetPlayer() => Game.Rebel;
    }
}