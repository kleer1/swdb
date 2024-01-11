using Game.Common.Interfaces;
using GameTest.Cards.Bases;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion.Bases
{
    public abstract class RebelAvailableBaseTest : AvailableBaseCardTest
    {
        public override IPlayer GetPlayer() => Game.Rebel;
    }
}