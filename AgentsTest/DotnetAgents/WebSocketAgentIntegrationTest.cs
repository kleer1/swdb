using Agents.Interfaces;
using Moq;
using System.Net.WebSockets;
using Game.State.Interfaces;
using Game.Actions;
using System.Text;
using Agents.DotnetAgents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AgentsTest.DotnetAgents
{
    [TestFixture]
    public class WebSocketAgentIntegrationTest
    {
        private static readonly string GameActionMsg = "This is a game action";
        private static readonly string GameStateMsg = "This is a game state";
        private const string WebSocketServerUrl = "ws://localhost:"; // Replace with your WebSocket server URL

        private Mock<IGameStateTranformer> _gameStateTranformerMock;
        private Mock<IGameActionConverter> _actionConverterMock;
        private Mock<IRewardGenerator> _rewardGeneratorMock;

        private PortManager _portManager = new PortManager();

        private WebSocketAgent _agent;

        private ILogger<WebSocketAgent> logger;

        [SetUp]
        public void Setup()
        {
            _gameStateTranformerMock = new Mock<IGameStateTranformer>();
            _actionConverterMock = new Mock<IGameActionConverter>();
            _rewardGeneratorMock = new Mock<IRewardGenerator>();

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            logger = factory.CreateLogger<WebSocketAgent>();
        }

        [Test]
        public async Task TestSelectAction()
        {
            var port = _portManager.GetNextPort();
            _agent = new WebSocketAgent(logger, port, _rewardGeneratorMock.Object, _gameStateTranformerMock.Object, _actionConverterMock.Object);
            await _agent.InitializeAsync();

            var expectedAction = new GameAction(Action.PassTurn);
            var gameStateMock = new Mock<IGameState>();
            _gameStateTranformerMock.Setup(x => x.TransformGameState(gameStateMock.Object)).Returns(GameStateMsg);
            _actionConverterMock.Setup(x => x.ConvertToGameAction(GameActionMsg)).Returns(expectedAction);
            //_rewardGeneratorMock.Setup(x => x.GenerateReward(It.IsAny<IGameState>(), It.IsAny<IGameState>())).Returns(5);
            gameStateMock.Setup(x => x.IsGameOver).Returns(true);

            using var clientWebSocket = await BuildWebSocket(port);
            SelectActionAsync(clientWebSocket, true);

            var action = await _agent.SelectActionAsync(gameStateMock.Object);
            That(action, Is.EqualTo(expectedAction));

            _agent.ShutdownAsync();

            await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
        }

        [Test]
        public async Task TestSelectActionTwice()
        {
            var port = _portManager.GetNextPort();
            _agent = new WebSocketAgent(logger, port, _rewardGeneratorMock.Object, _gameStateTranformerMock.Object, _actionConverterMock.Object);
            await _agent.InitializeAsync();

            var expectedAction = new GameAction(Action.PassTurn);
            var gameStateMock = new Mock<IGameState>();
            _gameStateTranformerMock.Setup(x => x.TransformGameState(gameStateMock.Object)).Returns(GameStateMsg);
            _actionConverterMock.Setup(x => x.ConvertToGameAction(GameActionMsg)).Returns(expectedAction);
            _rewardGeneratorMock.Setup(x => x.GenerateReward(It.IsAny<IGameState>(), It.IsAny<IGameState>())).Returns(5);
            gameStateMock.Setup(x => x.IsGameOver).Returns(true);

            using var clientWebSocket = await BuildWebSocket(port);
            SelectActionTwiceAsync(clientWebSocket, 5, true);

            var action = await _agent.SelectActionAsync(gameStateMock.Object);
            That(action, Is.EqualTo(expectedAction));

            action = await _agent.SelectActionAsync(gameStateMock.Object);
            That(action, Is.EqualTo(expectedAction));

            _agent.ShutdownAsync();

            await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
        }

        private async Task<ClientWebSocket> BuildWebSocket(int port)
        {
            var clientWebSocket = new ClientWebSocket();
            await clientWebSocket.ConnectAsync(new Uri(WebSocketServerUrl + port), CancellationToken.None);
            return clientWebSocket;
        }

        private async Task SelectActionAsync(ClientWebSocket clientWebSocket, bool done)
        {
            await SelectActionAsync(clientWebSocket, 0, done);
        }
        private async Task SelectActionAsync(ClientWebSocket clientWebSocket, int reward, bool done)
        {
            await Task.Delay(10);
            string msg = await ReceiveMessage(clientWebSocket);
            GameResponse? response = JsonConvert.DeserializeObject<GameResponse>(msg);
            That(response?.Observation, Is.EqualTo(GameStateMsg));
            That(response?.Reward, Is.EqualTo(reward));
            That(response?.Done, Is.EqualTo(done));

            await SendMessage(clientWebSocket, GameActionMsg);
        }

        private async Task SelectActionTwiceAsync(ClientWebSocket clientWebSocket, int reward, bool done)
        {
            await SelectActionAsync(clientWebSocket, 0, done);
            await SelectActionAsync(clientWebSocket, reward, done);
        }

        private async Task SendMessage(ClientWebSocket clientWebSocket, string message)
        {

            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await clientWebSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task<string> ReceiveMessage(ClientWebSocket clientWebSocket)
        {
            var buffer = new byte[1024];
            var result = await clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                return Encoding.UTF8.GetString(buffer, 0, result.Count);
            }

            throw new InvalidOperationException("Unexpected message type received");
        }
    }

    public class PortManager
    {
        private static int _port = 5000;

        public int GetNextPort()
        {
            return Interlocked.Increment(ref _port);
        }
    }
}