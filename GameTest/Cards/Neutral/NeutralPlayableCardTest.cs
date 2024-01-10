using GameTest.Cards.IPlayableCards;
using SWDB.Game.Common;

namespace GameTest.Cards.Neutral
{
    public abstract class NeutralIPlayableCardTest : IPlayableCardTest
    {
        public override Player GetPlayer() 
        {
            return Game.Empire;
        }
    }
}