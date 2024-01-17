namespace Agents.DotnetAgents
{
    public class GameResponse
    {
        public object? Observation { get; set; }
        public int Reward { get; set; }
        public bool Done { get; set; }

    }
}
