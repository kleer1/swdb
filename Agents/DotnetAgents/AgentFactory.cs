using Agents.DotnetAgents.BasicAgents;
using Agents.DotnetAgents.Models;
using Agents.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SWDB.Game.Common;

namespace Agents.DotnetAgents
{
    public class AgentFactory : IAgentFactory
    {
        private readonly ILogger<AgentFactory> _logger;
        private readonly IServiceProvider _serviceProvider;

        private ISet<int> availablePorts = Enumerable.Range(4000, 1000).ToHashSet();
        private ISet<int> usedPorts = new HashSet<int>();
        private readonly object portLock = new object();

        public AgentFactory(ILogger<AgentFactory> logger, IServiceProvider serviceProvder)
        {
            _logger = logger;
            _serviceProvider = serviceProvder;
        }

        public IAgent BuildAgent(AgentInfo info)
        {
            DotnetAgent agent = info.Agent;
            int port = GetPort();
            switch(agent)
            {
                case DotnetAgent.Basic:
                    return new AgentWithCallback(new BasicPythonAgent(_serviceProvider.GetRequiredService<ILogger<BasicPythonAgent>>(), port), () => ReleasePort(port));
                default:
                    throw new NotImplementedException();
            }
        }

        public IReadOnlyCollection<int> GetAvailablePorts()
        {
            IReadOnlyCollection<int> result;
            lock(portLock)
            {
                result = availablePorts.ToList();
            }
            return result;
        }

        public IReadOnlyCollection<int> GetUsedPorts()
        {
            IReadOnlyCollection<int> result;
            lock (portLock)
            {
                result = usedPorts.ToList();
            }
            return result;
        }

        private int GetPort()
        {
            lock (portLock)
            {
                if (availablePorts.Any())
                {
                    int port = availablePorts.First();
                    availablePorts.Remove(port);
                    usedPorts.Add(port);
                    return port;
                }
                else
                {
                    throw new Exception("All ports are in use");
                }
            }
        }

        private void ReleasePort(int port)
        {
            lock (portLock)
            {
                usedPorts.Remove(port);
                availablePorts.Add(port);
            }
        }
    }
}
