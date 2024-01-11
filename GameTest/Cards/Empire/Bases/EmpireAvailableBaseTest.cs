using Game.Common.Interfaces;
using GameTest.Cards.Bases;

namespace GameTest.Cards.Empire.Bases
{
    public abstract class EmpireAvailableBaseTest : AvailableBaseCardTest
    {
        public override IPlayer GetPlayer()
        {
            return Game.Empire;
        }
    }
}