using Agents.DotnetAgents.BasicAgents;
using Game.Actions;
using Game.Actions.Interfaces;
using Game.State.Interfaces;
using SWDB.Game;

namespace AgentsTest.DotnetAgents
{
    [TestFixture]
    public class BasicPythonAgentTest
    {
        private BasicPythonAgent agent;

        private PortManager portManager = new PortManager();

        [SetUp] 
        public async Task SetUp() 
        { 
            int port = portManager.GetNextPort();
            agent = new BasicPythonAgent(port);
            await agent.InitializeAsync();
        }

        [Test]
        public async Task TestEntireFlow()
        {
            SWDBGame game = new SWDBGame();
            IGameState state = game.GameState;
            IGameAction expectedAction;
            do
            {
                expectedAction = GetExpectedAction(state.ValidActions);
                IGameAction action = await agent.SelectActionAsync(game.GameState);
                That(action, Is.EqualTo(expectedAction));
                state = game.ApplyAction(action);

            } while (expectedAction.Action != SWDB.Game.Actions.Action.PassTurn);
            
            await agent.ShutdownAsync();
        }

        private static IGameAction GetExpectedAction(IReadOnlyCollection<IGameAction> gameActions)
        {
            var actionMap = BasicGameStateTransformer.BuildActionMap(gameActions);
            if (actionMap.TryGetValue(Action.PlayCard, out var play) && play.Count != 0)
            {
                return play.First();
            }
            else if (actionMap.TryGetValue(Action.AttackBase, out var atkBase) && atkBase.Count != 0)
            {
                return atkBase.First();
            }
            else if (actionMap.TryGetValue(Action.SelectAttacker, out var selectAtk) && selectAtk.Count != 0)
            {
                return selectAtk.First();
            }
            else if (actionMap.TryGetValue(Action.ConfirmAttackers, out var confirmAtk) && confirmAtk.Count != 0)
            {
                return confirmAtk.First();
            }
            else if (actionMap.TryGetValue(Action.PurchaseCard, out var purchase) && purchase.Count != 0)
            {
                return purchase.First();
            }
            else if (actionMap.TryGetValue(Action.DeclineAction, out var decline) && decline.Count != 0)
            {
                return decline.First();
            }
            return new GameAction(Action.PassTurn);
        }

    }
}
