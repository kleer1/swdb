using Agents.Interfaces;

namespace GameServer
{
    public interface IRunner
    {
        Task RunAsync();
        IAgent? Empire { get; set; }
        IAgent? Rebel { get; set; }
    }
}
