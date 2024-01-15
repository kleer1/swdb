namespace Agents.DotnetAgents.BasicAgents
{

    public class BasicPythonAgent(int port) : 
        PythonAgent(port, new BasicRewardGenerator(), new BasicGameStateTransformer(), new BasicGameActionConverter(), "basic_agent.py")
    {
    }
}
