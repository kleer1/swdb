using Agents.DotnetAgents;
using Agents.DotnetAgents.BasicAgents;
using Agents.DotnetAgents.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace AgentsTest.DotnetAgents
{
    [TestFixture]
    public class AgentFactoryTest
    {
        private ILogger<AgentFactory> logger;
        private AgentFactory agentFactory;


        [SetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            logger = factory.CreateLogger<AgentFactory>();

            agentFactory = new AgentFactory(logger, serviceProvider);
        }

        [Test]
        public void TestNotImplemented()
        {
            Throws<NotImplementedException>(() => agentFactory.BuildAgent(new AgentInfo { Agent = (DotnetAgent) 1000 }));
        }

        [Test]
        public void TestUsedAllPorts()
        {
            for (int i = 0; i < 1000; i++)
            {
                agentFactory.BuildAgent(new AgentInfo { Agent = DotnetAgent.Basic });
            }
            Throws<Exception>(() => agentFactory.BuildAgent(new AgentInfo { Agent = DotnetAgent.Basic }));
        }

        [Test]
        public void TestPortReleased()
        {
            That(agentFactory.GetAvailablePorts(), Has.Count.EqualTo(1000));
            That(agentFactory.GetUsedPorts(), Has.Count.EqualTo(0));
            using (var a = agentFactory.BuildAgent(new AgentInfo { Agent = DotnetAgent.Basic }))
            {
                That(agentFactory.GetAvailablePorts(), Has.Count.EqualTo(999));
                That(agentFactory.GetUsedPorts(), Has.Count.EqualTo(1));
                int port1 = agentFactory.GetUsedPorts().First();
                That(agentFactory.GetAvailablePorts(), Does.Not.Contain(port1));
                using (var b = agentFactory.BuildAgent(new AgentInfo { Agent = DotnetAgent.Basic }))
                {
                    That(agentFactory.GetAvailablePorts(), Has.Count.EqualTo(998));
                    That(agentFactory.GetUsedPorts(), Has.Count.EqualTo(2));
                    int port2 = agentFactory.GetUsedPorts().ElementAt(1);
                    That(agentFactory.GetAvailablePorts(), Does.Not.Contain(port1));
                    That(agentFactory.GetAvailablePorts(), Does.Not.Contain(port2));
                }
                That(agentFactory.GetAvailablePorts(), Has.Count.EqualTo(999));
                That(agentFactory.GetUsedPorts(), Has.Count.EqualTo(1));
                That(agentFactory.GetAvailablePorts(), Does.Not.Contain(port1));
            }
            That(agentFactory.GetAvailablePorts(), Has.Count.EqualTo(1000));
            That(agentFactory.GetUsedPorts(), Has.Count.EqualTo(0));
        }
    }
}
