using GameTest.Cards.IPlayableCards.Interfaces;

namespace GameTest.Cards.Empire.Units
{
    public abstract class EmpireTargetableCardTest : EmpireIPlayableCardTest, ITargetableCardTest
    {
        public abstract void AssertReward();
    }
}