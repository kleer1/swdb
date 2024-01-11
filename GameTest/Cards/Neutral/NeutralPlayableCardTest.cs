using Game.Common.Interfaces;
using GameTest.Cards.IPlayableCards;
using SWDB.Game.Common;

namespace GameTest.Cards.Neutral
{
    public abstract class NeutralIPlayableCardTest : IPlayableCardTest
    {
        public override IPlayer GetPlayer() 
        {
            return Game.Empire;
        }
    }
}