using Agents.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Agents.DotnetAgents
{
    public class PythonAgent : WebSocketAgent
    {
        private readonly Process? _process;
        private readonly string basePath;
        private readonly string pythonScriptsPath;
        private readonly Guid scripId;

        public PythonAgent(ILogger<PythonAgent> logger, int port, IRewardGenerator rewardGenerator, IGameStateTranformer gameStateTranformer,
            IGameActionConverter gameActionConverter, string pythonScriptName) :
                base(logger, port, rewardGenerator, gameStateTranformer, gameActionConverter)
        {
            basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
            pythonScriptsPath = Path.Combine(basePath, "PythonScripts");
            string cmd = "wsl";
            string args = $"bash -c \"python3 -m venv .venv && source .venv/bin/activate && pip3 install -r requirements.txt && python3 {pythonScriptName} --port {port}\"";

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = cmd,
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = pythonScriptsPath,
            };
            _process = new Process { StartInfo = startInfo };
            scripId = Guid.NewGuid();
        }

        public async override Task InitializeAsync()
        {

            await base.InitializeAsync();

            if (_process == null)
                throw new Exception("Could not create process");

            _process.Start();

            _process.OutputDataReceived += (sender, e) =>
            {
                using (LogContext.PushProperty("ScriptId", scripId))
                {
                    _logger.LogInformation(e.Data); // Event handler for stdout
                }
                
            };
            _process.ErrorDataReceived += (sender, e) =>
            {
                using (LogContext.PushProperty("ScriptId", scripId))
                {
                    _logger.LogError("Error: {Data}", e.Data); // Event handler for stderr
                }
            };

            // Begin asynchronous reading of stdout and stderr
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            _process?.Dispose();
            base.Dispose();
        }

        public override Task ShutdownAsync()
        {
            try
            {
                base.ShutdownAsync();
                _process?.WaitForExit();
                _process?.Dispose();
            }
            catch (InvalidOperationException e) when (e.Message == "No process is associated with this object.")
            {
                // means process already closed. just continue
            }

            return Task.CompletedTask;
        }
    }
}
