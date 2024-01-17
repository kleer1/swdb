using Agents.Interfaces;
using Game.Actions.Interfaces;
using Game.State.Interfaces;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using System.Text;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Agents.DotnetAgents
{
    public class WebSocketAgent : IAgent, IDisposable
    {
        protected readonly ILogger<WebSocketAgent> _logger;

        private IWebHost _host;
        private WebSocket _webSocket;
        private readonly IRewardGenerator _rewardGenerator;
        private readonly IGameStateTranformer _gameStateTranformer;
        private readonly IGameActionConverter _gameActionConverter;

        private TaskCompletionSource<IGameAction> _actionCompletionSource = new();
        private TaskCompletionSource<IGameState> _gameStateCompletionSource = new();
        private bool _isSelectActionCalled = false;
        private readonly object _selectActionLock = new();

        private bool _shouldShutdown = false;


        public WebSocketAgent(ILogger<WebSocketAgent> logger, int port, IRewardGenerator rewardGenerator, IGameStateTranformer gameStateTranformer,
            IGameActionConverter gameActionConverter)
        {
            _host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://127.0.0.1:" + port)
                .Configure(app =>
                {
                    app.UseWebSockets();
                    app.Run(HandleWebSocketAsync);
                })
                .Build();
            _rewardGenerator = rewardGenerator;
            _gameStateTranformer = gameStateTranformer;
            _gameActionConverter = gameActionConverter;
            _logger = logger;
        }

        public virtual async Task InitializeAsync()
        {
            try
            {
                // start the host
                await _host.StartAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError("Error starting host. {Ex}", ex);
            }
            
        }

        public async Task<IGameAction> SelectActionAsync(IGameState gameState)
        {
            _gameStateCompletionSource.SetResult(gameState);
            LockAndSet(_selectActionLock, ref _isSelectActionCalled);

            IGameAction action = await _actionCompletionSource.Task;
            _actionCompletionSource = new TaskCompletionSource<IGameAction>();
            return action;
        }

        public virtual async Task ShutdownAsync()
        {
            _shouldShutdown = true;
            await CloseWebSocket();

            await _host.StopAsync();
            Dispose();
        }

        private async Task CloseWebSocket()
        {
            try
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
            }
            catch (WebSocketException ex1) when (ex1.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
            {
                // this is happening sometimes. but we're closing anyway so not sure i care about it
            }
            catch (ConnectionAbortedException)
            {
                // this is happening sometimes. but we're closing anyway so not sure i care about it
            }
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            _shouldShutdown = true;
            _webSocket?.Dispose();
            _host?.Dispose();
        }

        private GameResponse BuildResponse(IGameState currentState, IGameState previousState)
        {
            return new GameResponse
            {
                Observation = _gameStateTranformer.TransformGameState(currentState),
                Reward = _rewardGenerator.GenerateReward(previousState, currentState),
                Done = currentState.IsGameOver
            };
        }

        private GameResponse BuildResponse(IGameState currentState)
        {
            return new GameResponse
            {
                Observation = _gameStateTranformer.TransformGameState(currentState),
                Reward = 0,
                Done = currentState.IsGameOver
            };
        }

        private async Task HandleWebSocketAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                try
                {
                    IGameState? previousState = null;

                    _webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    // get first move. there will be no reward for this
                    previousState = await GetAction(previousState);

                    while (!_shouldShutdown && _webSocket.State == WebSocketState.Open)
                    {

                        if (CheckLockedBoolCalled(_selectActionLock, ref _isSelectActionCalled))
                        {
                            previousState = await GetAction(previousState);

                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }

            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }

        private async Task<IGameState> GetAction(IGameState? previousState)
        {
            IGameState currentState = await _gameStateCompletionSource.Task;
            _gameStateCompletionSource = new TaskCompletionSource<IGameState>();
            string response;
            if (previousState == null)
            {
                response = JsonConvert.SerializeObject(BuildResponse(currentState));
            }
            else
            {
                response = JsonConvert.SerializeObject(BuildResponse(currentState, previousState));
            }
            await SendWebSocketMessageAsync(_webSocket, response);
            await Task.Delay(10);
            // get action from python
            string action = await ReceiveWebSocketMessageAsync(_webSocket);
            _actionCompletionSource.SetResult(_gameActionConverter.ConvertToGameAction(action));
            return currentState;
        }

        private static async Task SendWebSocketMessageAsync(WebSocket webSocket, string message)
        {
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
            await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

        }

        private async Task<string> ReceiveWebSocketMessageAsync(WebSocket webSocket)
        {
            try
            {
                using var ms = new MemoryStream();
                WebSocketReceiveResult result;
                do
                {
                    var messageBuffer = WebSocket.CreateClientBuffer(1024, 16);
                    result = await webSocket.ReceiveAsync(messageBuffer, CancellationToken.None);
                    ms.Write(messageBuffer.Array, messageBuffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await CloseWebSocket();
                }
                return "Could not find message";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return "Could not parse message";
            }


        }

        private static bool CheckLockedBoolCalled(object objectLock, ref bool toCheck)
        {
            lock (objectLock)
            {
                if (toCheck)
                {
                    toCheck = false;
                    return true;
                }
                return false;
            }

        }

        private static void LockAndSet(object objectLock, ref bool toSet)
        {
            lock (objectLock)
            {
                toSet = true;
            }
        }
    }
}
