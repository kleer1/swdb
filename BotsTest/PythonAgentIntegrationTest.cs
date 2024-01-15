using Bots;
using Bots.Interfaces;
using Game.Actions;
using Game.State.Interfaces;
using Moq;

namespace BotsTest
{
    [TestFixture]
    public class PythonAgentIntegrationTest
    {
        private const string GameActionMsg = "This is a game action";
        private const string GameStateMsg = "This is a game state";
        private const string PythonScript = "test_agent.py";


        private Mock<IGameStateTranformer> _gameStateTranformerMock;
        private Mock<IGameActionConverter> _actionConverterMock;
        private Mock<IRewardGenerator> _rewardGeneratorMock;

        private PortManager _portManager = new PortManager();

        private PythonAgent _agent;

        [SetUp]
        public void Setup()
        {
            _gameStateTranformerMock = new Mock<IGameStateTranformer>();
            _actionConverterMock = new Mock<IGameActionConverter>();
            _rewardGeneratorMock = new Mock<IRewardGenerator>();
        }

        [Test]
        public async Task TestEntireFlow()
        {
            Console.Write(GameActionMsg);
            var port = _portManager.GetNextPort();
            _agent = new PythonAgent(port, _rewardGeneratorMock.Object, _gameStateTranformerMock.Object, _actionConverterMock.Object, PythonScript);
            await _agent.InitializeAsync();

            var expectedAction = new GameAction(Action.DeclineAction);
            var gameStateMock = new Mock<IGameState>();
            _gameStateTranformerMock.Setup(x => x.TransformGameState(gameStateMock.Object)).Returns(GameStateMsg);
            _actionConverterMock.Setup(x => x.ConvertToGameACtion(GameActionMsg)).Returns(expectedAction);
            _rewardGeneratorMock.Setup(x => x.GenerateReward(gameStateMock.Object, gameStateMock.Object)).Returns(10);

            var action = await _agent.SelectActionAsync(gameStateMock.Object);
            That(action, Is.EqualTo(expectedAction));

            await _agent.PostActionProcessingAsync(gameStateMock.Object, gameStateMock.Object);

            bool resp = await _agent.ShouldStopGameAsync();
            That(resp, Is.False);

            await _agent.ShutdownAsync();
            _agent.Dispose();
        }
    }
}
