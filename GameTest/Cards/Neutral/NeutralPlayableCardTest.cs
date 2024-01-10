using GameTest.Cards.PlayableCards;
using SWDB.Game.Common;

namespace GameTest.Cards.Neutral
{
    public abstract class NeutralPlayableCardTest : PlayableCardTest
    {
        public override Player GetPlayer() 
        {
            return Game.Empire;
        }
    }
}