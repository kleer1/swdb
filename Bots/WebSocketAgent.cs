using Bots.Interfaces;
using Game.Actions.Interfaces;
using Game.State.Interfaces;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using System.Text;
using System;
using Microsoft.AspNetCore.Connections;

namespace Bots
{
    public class WebSocketAgent : IAgent, IDisposable
    {
        private static readonly string Delim = "!";

        private IWebHost _host;
        private WebSocket _webSocket;
        private readonly IRewardGenerator _rewardGenerator;
        private readonly IGameStateTranformer _gameStateTranformer;
        private readonly IGameActionConverter _gameActionConverter;

        private TaskCompletionSource<IGameAction> _actionCompletionSource = new();
        private TaskCompletionSource<IGameState> _gameStateCompletionSource = new();
        private TaskCompletionSource<int> _rewardCompletionSource = new();
        private TaskCompletionSource<bool> _stopGameCompletionSource = new();

        private bool _isSelectActionCalled = false;
        private readonly object _selectActionLock = new();
        private bool _isPostProcesingCalled = false;
        private readonly object _postProcessingLock = new();
        private bool _isPostProcesingComplete = false;
        private readonly object _postProcessingCompleteLock = new();
        private bool _isShouldShutdownCalled = false;
        private readonly object _shouldShutdownLock = new();

        private bool _shouldShutdown = false;

        public WebSocketAgent(int port, IRewardGenerator rewardGenerator, IGameStateTranformer gameStateTranformer, 
            IGameActionConverter gameActionConverter)
        {
            _host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://localhost:" + port)
                .Configure(app => {
                    app.UseWebSockets();
                    app.Run(HandleWebSocketAsync);
                })
                .Build();
            _rewardGenerator = rewardGenerator;
            _gameStateTranformer = gameStateTranformer;
            _gameActionConverter = gameActionConverter;
        }

        public virtual async Task InitializeAsync()
        {
            // start the host
            await _host.StartAsync();
        }

        public async Task PostActionProcessingAsync(IGameState oldState, IGameState newState)
        {
            _rewardCompletionSource.SetResult(_rewardGenerator.GenerateReward(oldState, newState));
            LockAndSet(_postProcessingLock, ref _isPostProcesingCalled);
            while(!CheckLockedBoolCalled(_postProcessingCompleteLock, ref _isPostProcesingComplete)) 
            {
                await Task.Delay(10);
            }
            return;
        }

        public async Task<IGameAction> SelectActionAsync(IGameState gameState)
        {
            _gameStateCompletionSource.SetResult(gameState);
            LockAndSet(_selectActionLock, ref _isSelectActionCalled);

            IGameAction action = await _actionCompletionSource.Task;
            _actionCompletionSource = new TaskCompletionSource<IGameAction>();
            return action;
        }

        public async Task<bool> ShouldStopGameAsync()
        {
            LockAndSet(_shouldShutdownLock, ref _isShouldShutdownCalled);
            bool shutdown = await _stopGameCompletionSource.Task;
            _stopGameCompletionSource = new TaskCompletionSource<bool>();
            return shutdown;
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

        private async Task HandleWebSocketAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                try
                {
                    _webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    while (!_shouldShutdown && _webSocket.State == WebSocketState.Open)
                    {

                        if (CheckLockedBoolCalled(_selectActionLock, ref _isSelectActionCalled))
                        {
                            IGameState gameState = await _gameStateCompletionSource.Task;
                            _gameStateCompletionSource = new TaskCompletionSource<IGameState>();
                            // send game state to python
                            await SendWebSocketMessageAsync(_webSocket, _gameStateTranformer.TransformGameState(gameState));
                            await Task.Delay(10);
                            // get action from python
                            string action = await ReceiveWebSocketMessageAsync(_webSocket);
                            _actionCompletionSource.SetResult(_gameActionConverter.ConvertToGameACtion(action));

                        }

                        if (CheckLockedBoolCalled(_postProcessingLock, ref _isPostProcesingCalled))
                        {
                            // generate reward value and send to python
                            int reward = await _rewardCompletionSource.Task;
                            _rewardCompletionSource = new TaskCompletionSource<int>();
                            await SendWebSocketMessageAsync(_webSocket, "Reward: " + reward.ToString());
                            LockAndSet(_postProcessingCompleteLock, ref _isPostProcesingComplete);
                        }

                        if (CheckLockedBoolCalled(_shouldShutdownLock, ref _isShouldShutdownCalled))
                        {
                            // ask python if the training is done for them
                            await SendWebSocketMessageAsync(_webSocket, "Should stop");
                            string msg = await ReceiveWebSocketMessageAsync(_webSocket);
                            if (msg.ToLower() == "no")
                            {
                                _stopGameCompletionSource.SetResult(false);
                            }
                            else if (msg.ToLower() == "yes")
                            {
                                _stopGameCompletionSource.SetResult(true);
                            }
                            else
                            {
                                throw new Exception("Unexpected ShouldStop response received: " + msg);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }

        private static async Task SendWebSocketMessageAsync(WebSocket webSocket, string message)
        {
                var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            
        }

        private static async Task<string> ReceiveWebSocketMessageAsync(WebSocket webSocket)
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
                return "Could not find message";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
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
            lock (objectLock) { 
                toSet = true;
            }
        }
    }
}
