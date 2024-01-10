using GameTest.Cards.IPlayableCards.Interfaces;

namespace GameTest.Cards.Rebellion.Units
{
    public abstract class RebelTargetableCardTest : RebelIPlayableCardTest, ITargetableCardTest
    {
        public abstract void AssertReward();
    }
}