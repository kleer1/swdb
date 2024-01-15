using Game.Actions.Interfaces;
using Game.State.Interfaces;

namespace Agents.Interfaces
{
    public interface IAgent
    {
        Task InitializeAsync();
        Task<IGameAction> SelectActionAsync(IGameState gameState);
        Task PostActionProcessingAsync(IGameState oldState, IGameState newState);
        Task<bool> ShouldStopGameAsync();
        Task ShutdownAsync();
    }
}
