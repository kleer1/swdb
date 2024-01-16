using Microsoft.Extensions.Logging;

namespace Agents.DotnetAgents.BasicAgents
{

    public class BasicPythonAgent(ILogger<BasicPythonAgent> logger, int port) : 
        PythonAgent(logger, port, new BasicRewardGenerator(), new BasicGameStateTransformer(), new BasicGameActionConverter(), "basic_agent.py")
    {
    }
}
