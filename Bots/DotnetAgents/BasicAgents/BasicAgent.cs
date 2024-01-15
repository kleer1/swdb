namespace Bots.DotnetAgents.BasicAgents
{

    public class BasicAgent(int port) : 
        PythonAgent(port, new BasicRewardGenerator(), new BasicGameStateTransformer(), new BasicGameActionConverter(), "basic_agent.py")
    {
    }
}
