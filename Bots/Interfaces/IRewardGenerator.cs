using Game.State.Interfaces;

namespace Bots.Interfaces
{
    public interface IRewardGenerator
    {
        int GenerateReward(IGameState oldState, IGameState newState);
    }
}
