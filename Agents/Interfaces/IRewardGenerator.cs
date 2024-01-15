using Game.State.Interfaces;

namespace Agents.Interfaces
{
    public interface IRewardGenerator
    {
        int GenerateReward(IGameState oldState, IGameState newState);
    }
}
