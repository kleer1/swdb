using Agents.DotnetAgents.Models;

namespace GameServer.Models
{
    public class StartGameRequest
    {
        public required AgentInfo Empire {  get; set; }
        public required AgentInfo Rebel { get; set; }
    }
}
