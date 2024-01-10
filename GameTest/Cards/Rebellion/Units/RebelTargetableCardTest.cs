using GameTest.Cards.PlayableCards.Interfaces;

namespace GameTest.Cards.Rebellion.Units
{
    public abstract class RebelTargetableCardTest : RebelPlayableCardTest, ITargetableCardTest
    {
        public abstract void AssertReward();
    }
}