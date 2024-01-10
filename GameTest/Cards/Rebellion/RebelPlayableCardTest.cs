using GameTest.Cards.IPlayableCards;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion
{
    public abstract class RebelIPlayableCardTest : IPlayableCardTest
    {
        public override Player GetPlayer() 
        {
            return Game.Rebel;
        }
    }
}