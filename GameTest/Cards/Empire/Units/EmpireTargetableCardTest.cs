using GameTest.Cards.PlayableCards.Interfaces;

namespace GameTest.Cards.Empire.Units
{
    public abstract class EmpireTargetableCardTest : EmpirePlayableCardTest, ITargetableCardTest
    {
        public abstract void AssertReward();
    }
}