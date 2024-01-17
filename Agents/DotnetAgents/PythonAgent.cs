using Agents.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Agents.DotnetAgents
{
    public class PythonAgent : WebSocketAgent
    {
        private readonly Process? _process;
        private readonly string basePath;
        private readonly string pythonScriptsPath;
        private readonly IList<string> commandsToExecute;
        private readonly Guid scripId;

        public PythonAgent(ILogger<PythonAgent> logger, int port, IRewardGenerator rewardGenerator, IGameStateTranformer gameStateTranformer,
            IGameActionConverter gameActionConverter, string pythonScriptName) :
                base(logger, port, rewardGenerator, gameStateTranformer, gameActionConverter)
        {
            basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
            pythonScriptsPath = Path.Combine(basePath, "PythonScripts\\");
            const string cmd = "cmd.exe";
            const string args = "";
            commandsToExecute = new List<string>
            {
                "python.exe -m venv .venv",
                ".venv\\Scripts\\activate",
                "pip install -r requirements.txt",
                $"python.exe {pythonScriptName} --port {port}"
            };

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = false,
                Arguments = args,
                FileName = cmd,
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

            using var sw = _process.StandardInput;
            if (sw.BaseStream.CanWrite)
            {
                foreach (var command in commandsToExecute)
                {
                    sw.WriteLine(command);
                }
                sw.Flush();
                sw.Close();
            }
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
