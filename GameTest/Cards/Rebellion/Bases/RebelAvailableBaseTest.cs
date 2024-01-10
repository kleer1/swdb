using GameTest.Cards.Bases;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Bases
{
    public abstract class RebelAvailableBaseTest : AvailableBaseCardTest
    {
        public override Player GetPlayer() => Game.Rebel;
    }
}