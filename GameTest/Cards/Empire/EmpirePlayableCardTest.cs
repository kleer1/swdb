using GameTest.Cards.PlayableCards;
using SWDB.Game.Common;

namespace GameTest.Cards.Empire
{
    public abstract class EmpirePlayableCardTest : PlayableCardTest
    {
        public override Player GetPlayer()
        {
           return Game.Empire;
        }
    }
}