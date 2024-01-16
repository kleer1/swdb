namespace Agents.DotnetAgents.Models
{
    public class AgentInfo
    {
        public required DotnetAgent Agent { get; set; }
    }

    public enum DotnetAgent
    {
        Basic,
    }
}
