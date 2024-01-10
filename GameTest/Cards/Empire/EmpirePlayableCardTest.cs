using GameTest.Cards.IPlayableCards;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire
{
    public abstract class EmpireIPlayableCardTest : IPlayableCardTest
    {
        public override Player GetPlayer()
        {
           return Game.Empire;
        }
    }
}