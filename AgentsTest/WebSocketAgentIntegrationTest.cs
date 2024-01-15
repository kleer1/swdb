using Agents;
using Agents.Interfaces;
using Moq;
using System.Net.WebSockets;
using Game.State.Interfaces;
using Game.Actions;
using System.Text;

namespace BotsTest
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

        [SetUp]
        public void Setup()
        {
            _gameStateTranformerMock = new Mock<IGameStateTranformer>();
            _actionConverterMock = new Mock<IGameActionConverter>();
            _rewardGeneratorMock = new Mock<IRewardGenerator>();
        }

        [Test]
        public async Task TestSelectAction()
        {
            var port = _portManager.GetNextPort();
            _agent = new WebSocketAgent(port, _rewardGeneratorMock.Object, _gameStateTranformerMock.Object, _actionConverterMock.Object);
            await _agent.InitializeAsync();

            var expectedAction = new GameAction(Action.PassTurn);
            var gameStateMock = new Mock<IGameState>();
            _gameStateTranformerMock.Setup(x => x.TransformGameState(gameStateMock.Object)).Returns(GameStateMsg);
            _actionConverterMock.Setup(x => x.ConvertToGameACtion(GameActionMsg)).Returns(expectedAction);

            using var clientWebSocket = await BuildWebSocket(port);
            SelectActionAsync(clientWebSocket);

            var action = await _agent.SelectActionAsync(gameStateMock.Object);
            That(action, Is.EqualTo(expectedAction));

            _agent.ShutdownAsync();

            await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
        }

        [Test]
        public async Task TestPostProcessing()
        {
            var port = _portManager.GetNextPort();
            _agent = new WebSocketAgent(port, _rewardGeneratorMock.Object, _gameStateTranformerMock.Object, _actionConverterMock.Object);
            await _agent.InitializeAsync();

            var gameStateMock = new Mock<IGameState>();
            _rewardGeneratorMock.Setup(x => x.GenerateReward(gameStateMock.Object, gameStateMock.Object)).Returns(5);

            using var clientWebSocket = await BuildWebSocket(port);
            PostProcessingAsync(clientWebSocket, "Reward: 5");

            await _agent.PostActionProcessingAsync(gameStateMock.Object, gameStateMock.Object);

            clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);

            await _agent.ShutdownAsync();
        }

        [Test]
        public async Task TestShouldStopGame()
        {
            var port = _portManager.GetNextPort();
            _agent = new WebSocketAgent(port, _rewardGeneratorMock.Object, _gameStateTranformerMock.Object, _actionConverterMock.Object);
            await _agent.InitializeAsync();

            using var clientWebSocket = await BuildWebSocket(port);
            ShouldStopGameAsync(clientWebSocket, true);

            bool resp = await _agent.ShouldStopGameAsync();
            That(resp, Is.True);

            _agent.ShutdownAsync();

            await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
        }

        [Test]
        public async Task TestShouldNotStopGame()
        {
            var port = _portManager.GetNextPort();
            _agent = new WebSocketAgent(port, _rewardGeneratorMock.Object, _gameStateTranformerMock.Object, _actionConverterMock.Object);
            await _agent.InitializeAsync();

            using var clientWebSocket = await BuildWebSocket(port);
            ShouldStopGameAsync(clientWebSocket, false);

            bool resp = await _agent.ShouldStopGameAsync();
            That(resp, Is.False);

            _agent.ShutdownAsync();

            await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
        }

        [Test]
        public async Task TestEntireFlow()
        {
            var port = _portManager.GetNextPort();
            _agent = new WebSocketAgent(port, _rewardGeneratorMock.Object, _gameStateTranformerMock.Object, _actionConverterMock.Object);
            await _agent.InitializeAsync();

            var expectedAction = new GameAction(Action.DeclineAction);
            var gameStateMock = new Mock<IGameState>();
            _gameStateTranformerMock.Setup(x => x.TransformGameState(gameStateMock.Object)).Returns(GameStateMsg);
            _actionConverterMock.Setup(x => x.ConvertToGameACtion(GameActionMsg)).Returns(expectedAction);
            _rewardGeneratorMock.Setup(x => x.GenerateReward(gameStateMock.Object, gameStateMock.Object)).Returns(10);

            using var clientWebSocket = await BuildWebSocket(port);
            RunAll(clientWebSocket, "Reward: 10", true);

            var action = await _agent.SelectActionAsync(gameStateMock.Object);
            That(action, Is.EqualTo(expectedAction));

            await _agent.PostActionProcessingAsync(gameStateMock.Object, gameStateMock.Object);

            bool resp = await _agent.ShouldStopGameAsync();
            That(resp, Is.True);

            _agent.ShutdownAsync();

            await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
        }

        [Test]
        public async Task TestEntireFlowTwice()
        {
            var port = _portManager.GetNextPort();
            _agent = new WebSocketAgent(port, _rewardGeneratorMock.Object, _gameStateTranformerMock.Object, _actionConverterMock.Object);
            await _agent.InitializeAsync();

            var expectedAction1 = new GameAction(Action.DeclineAction);
            var expectedAction2 = new GameAction(Action.PlayCard, 1);
            var gameStateMock = new Mock<IGameState>();
            _gameStateTranformerMock.Setup(x => x.TransformGameState(gameStateMock.Object)).Returns(GameStateMsg);
            _actionConverterMock.SetupSequence(x => x.ConvertToGameACtion(GameActionMsg)).Returns(expectedAction1).Returns(expectedAction2);
            _rewardGeneratorMock.SetupSequence(x => x.GenerateReward(gameStateMock.Object, gameStateMock.Object)).Returns(50).Returns(0);

            using var clientWebSocket = await BuildWebSocket(port);
            RunAllTwice(clientWebSocket, "Reward: 50", false, "Reward: 0", true);

            var action = await _agent.SelectActionAsync(gameStateMock.Object);
            That(action, Is.EqualTo(expectedAction1));

            await _agent.PostActionProcessingAsync(gameStateMock.Object, gameStateMock.Object);

            bool resp = await _agent.ShouldStopGameAsync();
            That(resp, Is.False);

            action = await _agent.SelectActionAsync(gameStateMock.Object);
            That(action, Is.EqualTo(expectedAction2));

            await _agent.PostActionProcessingAsync(gameStateMock.Object, gameStateMock.Object);

            resp = await _agent.ShouldStopGameAsync();
            That(resp, Is.True);

            _agent.ShutdownAsync();

            await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
        }

        private async Task<ClientWebSocket> BuildWebSocket(int port)
        {
            var clientWebSocket = new ClientWebSocket();
            await clientWebSocket.ConnectAsync(new Uri(WebSocketServerUrl + port), CancellationToken.None);
            return clientWebSocket;
        }

        private async Task SelectActionAsync(ClientWebSocket clientWebSocket)
        {
            await Task.Delay(10);
            string msg = await ReceiveMessage(clientWebSocket);
            That(msg, Is.EqualTo(GameStateMsg));

            await SendMessage(clientWebSocket, GameActionMsg);
        }

        private async Task PostProcessingAsync(ClientWebSocket clientWebSocket, string expectedMsg)
        {
            await Task.Delay(10);
            string msg = await ReceiveMessage(clientWebSocket);
            That(msg, Is.EqualTo(expectedMsg));
        }

        private async Task ShouldStopGameAsync(ClientWebSocket clientWebSocket, bool shouldStop)
        {
            await Task.Delay(10);
            string msg = await ReceiveMessage(clientWebSocket);
            That(msg, Is.EqualTo("Should stop"));

            if (shouldStop)
            {
                await SendMessage(clientWebSocket, "Yes");
            } 
            else
            {
                await SendMessage(clientWebSocket, "NO");
            }
        }

        private async Task RunAll(ClientWebSocket clientWebSocket, string expectedMsg, bool shouldStop)
        {
            await SelectActionAsync(clientWebSocket);
            await PostProcessingAsync(clientWebSocket, expectedMsg);
            await ShouldStopGameAsync(clientWebSocket, shouldStop);
        }

        private async void RunAllTwice(ClientWebSocket clientWebSocket, string expectedMsg1, bool shouldStop1, string expectedMsg2, bool shouldStop2)
        {
            await RunAll(clientWebSocket, expectedMsg1, shouldStop1);
            await RunAll(clientWebSocket, expectedMsg2, shouldStop2);
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