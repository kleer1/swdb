using Agents.Interfaces;
using Game.Actions.Interfaces;
using Game.State.Interfaces;

namespace Agents.DotnetAgents.Models
{
    public class AgentWithCallback : IDisposable, IAgent
    {
        private readonly IAgent _agent;
        private readonly System.Action _action;

        public AgentWithCallback(IAgent agent, System.Action callback)
        {
            _agent = agent;
            _action = callback;
        }

        public void Dispose()
        {
            _agent.Dispose();
            _action.Invoke();
        }

        public Task InitializeAsync()
        {
            return _agent.InitializeAsync();
        }

        public Task<IGameAction> SelectActionAsync(IGameState gameState)
        {
            return _agent.SelectActionAsync(gameState);
        }

        public Task ShutdownAsync()
        {
            return _agent.ShutdownAsync();
        }
    }
}
