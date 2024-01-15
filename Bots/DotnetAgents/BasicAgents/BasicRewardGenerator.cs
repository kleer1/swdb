using Bots.Interfaces;
using Game.State.Interfaces;

namespace Bots.DotnetAgents.BasicAgents
{
    internal class BasicRewardGenerator : IRewardGenerator
    {
        public int GenerateReward(IGameState oldState, IGameState newState)
        {
            // basic agent doesn not learn. no reward necessary.
            return 0;
        }
    }
}
