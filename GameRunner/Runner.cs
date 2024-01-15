using Bots.Interfaces;
using Game.Actions.Interfaces;
using Game.State.Interfaces;
using SWDB.Game;
using SWDB.Game.Common;

namespace GameRunner
{
    public class Runner
    {
        private SWDBGame Game {  get; set; }
        private IAgent Empire { get; set; }
        private IAgent Rebel { get; set; }
        private bool ShouldExit { get; set; } = false;

        public Runner(IAgent empire, IAgent rebel)
        {
            Empire = empire;
            Rebel = rebel;
            Game = new SWDBGame();
        }

        public async void RunAsync()
        {
            IGameState gameState = Game.GameState;
            await Empire.InitializeAsync();
            await Rebel.InitializeAsync();
            Console.WriteLine(gameState);
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
                    throw new Exception("Current players action is neither empire or rebel");
                }
                gameState = await ApplyActionAsync(currentAgent, gameState);
                if (await currentAgent.ShouldStopGameAsync())
                {
                    ShouldExit = true;
                }
                Console.WriteLine(gameState);
                if (ShouldExit) break;
            }
            await Empire.ShutdownAsync();
            await Rebel.ShutdownAsync();
        }

        private async Task<IGameState> ApplyActionAsync(IAgent agent, IGameState gameState)
        {
            IGameAction selectedAction = await agent.SelectActionAsync(gameState);
            IGameState nextState = Game.ApplyAction(selectedAction);
            await agent.PostActionProcessingAsync(gameState, nextState);
            ShouldExit = await agent.ShouldStopGameAsync();
            return nextState;
        }
    }
}
