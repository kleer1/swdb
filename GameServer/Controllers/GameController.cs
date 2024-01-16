using Agents.Interfaces;
using GameServer.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GameServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;
        private readonly IAgentFactory _agentFactory;
        private readonly IRunner _runner;

        public GameController(ILogger<GameController> logger, IAgentFactory agentFactory, IRunner runner)
        {
            _logger = logger;
            _agentFactory = agentFactory;
            _runner = runner;
        }

        [HttpPost("StartGame")]
        public async Task<IActionResult> StartGame([FromBody] StartGameRequest request)
        {
            _logger.LogInformation("Received start game request: {Request}", request);
            _runner.Empire = _agentFactory.BuildAgent(request.Empire);
            _runner.Rebel = _agentFactory.BuildAgent(request.Rebel);
            _runner.RunAsync();
            return Ok();
        }
    }
}
