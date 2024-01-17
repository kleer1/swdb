using Agents.Interfaces;
using Game.Actions.Interfaces;
using Game.State.Interfaces;
using GameServer;
using Serilog.Context;
using SWDB.Game;
using SWDB.Game.Common;

namespace GameRunner
{
    public class Runner : IRunner
    {
        private readonly ILogger<Runner> _logger;

        private SWDBGame Game {  get; set; }
        public IAgent? Empire { get; set; }
        public IAgent? Rebel { get; set; }
        private bool ShouldExit { get; set; } = false;

        public Runner(ILogger<Runner> logger)
        {
            Game = new SWDBGame();
            _logger = logger;
        }

        public async Task RunAsync()
        {
            if (Empire == null || Rebel == null)
            {
                _logger.LogWarning("Could not run game. At least one of the agents was null. {Empire}, {Rebel}", Empire, Rebel);
                return;
            }
            using var cxt =  LogContext.PushProperty("Game", Guid.NewGuid());
            IGameState gameState = Game.GameState;
            await Empire.InitializeAsync();
            await Rebel.InitializeAsync();
            //_logger.LogInformation("{GameState}", gameState);
            // Empire goes first
            while (!Game.IsGameOver) 
            {
                IAgent currentAgent;
                if (Game.CurrentPlayersAction == Faction.empire)
                {
                    currentAgent = Empire;
                } else if (Game.CurrentPlayersAction == Faction.rebellion) 
                {
                    currentAgent = Rebel;
                } else
                {
                    _logger.LogError("Current players action is neither empire or rebel");
                    return;
                }
                using (LogContext.PushProperty("Faction", currentAgent == Empire ? "Empire" : "Rebel"))
                {
                    gameState = await ApplyActionAsync(currentAgent, gameState);
                }
                //_logger.LogInformation("Current game state: {GameState}", gameState);
                if (ShouldExit) break;
            }
            if (Game.IsGameOver)
            {
                _logger.LogInformation("Winner was {Winner}", Game.GetWinner());
            }
            await Empire.ShutdownAsync();
            await Rebel.ShutdownAsync();
            Empire.Dispose();
            Rebel.Dispose();
        }

        private async Task<IGameState> ApplyActionAsync(IAgent agent, IGameState gameState)
        {
            IGameAction selectedAction = await agent.SelectActionAsync(gameState);
            IGameState nextState = Game.ApplyAction(selectedAction);
            return nextState;
        }
    }
}
