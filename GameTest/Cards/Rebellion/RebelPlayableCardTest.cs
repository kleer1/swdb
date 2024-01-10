using GameTest.Cards.PlayableCards;
using SWDB.Game.Common;

namespace GameTest.Cards.Rebellion
{
    public abstract class RebelPlayableCardTest : PlayableCardTest
    {
        public override Player GetPlayer() 
        {
            return Game.Rebel;
        }
    }
}