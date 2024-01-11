using Game.Common.Interfaces;
using GameTest.Cards.IPlayableCards;

namespace GameTest.Cards.Empire
{
    public abstract class EmpireIPlayableCardTest : IPlayableCardTest
    {
        public override IPlayer GetPlayer()
        {
           return Game.Empire;
        }
    }
}