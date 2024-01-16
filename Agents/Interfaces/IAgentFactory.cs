using Agents.DotnetAgents.Models;
using Microsoft.Extensions.Logging;
using SWDB.Game.Common;

namespace Agents.Interfaces
{
    public interface IAgentFactory
    {
        IAgent BuildAgent(AgentInfo info);
    }
}
