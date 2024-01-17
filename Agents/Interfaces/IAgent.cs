using Game.Actions.Interfaces;
using Game.State.Interfaces;

namespace Agents.Interfaces
{
    public interface IAgent : IDisposable
    {
        Task InitializeAsync();
        Task<IGameAction> SelectActionAsync(IGameState gameState);
        Task ShutdownAsync();
    }
}
