using GameTest.Cards.Bases;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire.Bases
{
    public abstract class EmpireAvailableBaseTest : AvailableBaseCardTest
    {
        public override Player GetPlayer()
        {
            return Game.Empire;
        }
    }
}