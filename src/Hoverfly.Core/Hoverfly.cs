namespace Hoverfly.Core
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Text;
    using System.Threading;

    using Configuration;
    using Logging;
    using Model;
    using Resources;

    public class Hoverfly : IDisposable
    {
        private const int BOOT_TIMEOUT_SECONDS = 10;
        private const int RETRY_BACKOFF_INTERVAL_MS = 100;

        private const int KILL_PROCESS_TIMEOUT = 5000;

        private const string HOVERFLY_EXE = "hoverfly.exe";

        private readonly HoverflyConfig _hoverflyConfig;

        private readonly IHoverflyClient _hoverflyClient;

        private readonly ILog _logger;

        private readonly HoverflyMode _hoverflyMode;

        private Process _hoverflyProcess;

        bool _disposed = false;

        /// <summary>
        /// Provide access to Hoverfly to start and stop simulation or capture HTTP calls.
        /// </summary>
        /// <param name="hoverflyMode">The mode hoverfly should be started in. <see cref="HoverflyMode"/></param>
        /// <param name="config">Hoverfly configurations. <see cref="HoverflyConfig"/></param>
        /// <param name="loggerFactory">A logger factory for creating a logger to log messages.</param>
        /// <param name="hoverflyClient">Hoverfly client, by default the <see cref="HoverflyClient"/> is used to accessing the Hoverfly process REST API.</param>
        public Hoverfly(
            HoverflyMode hoverflyMode,
            HoverflyConfig config = null,
            ILoggerFactory loggerFactory = null,
            IHoverflyClient hoverflyClient = null)
        {
            _hoverflyMode = hoverflyMode;

            _hoverflyConfig = config ?? HoverflyConfig.Config();

            _logger = loggerFactory?.Create(GetType().Name);

            _hoverflyClient = hoverflyClient ?? new HoverflyClient(
                                                         new Uri($"{_hoverflyConfig.RemoteHost}:{_hoverflyConfig.AdminPort}"),
                                                         _logger);
        }

        /// <summary>
        /// Starts the hoverfly process.
        /// </summary>
        public void Start()
        {
            if (!_hoverflyConfig.IsRemoteInstance)
                StartHoverflyProcess();

            WaitForHoverflyToBecomeHealthy();
            SetProxySystemProperties();
        }

        /// <summary>
        /// Stop the hoverfly process.
        /// </summary>
        /// <exception cref="TimeoutException">
        /// Thrown when the hoverfly process is still running after trying to kill it.
        /// Default timeout is 5 seconds.
        /// </exception>
        public void Stop()
        {
            WebRequest.DefaultWebProxy = null;

            _logger?.Info("Destroying hoverfly process");

            if (_hoverflyProcess == null)
                return;

            _hoverflyProcess.Kill();
            _hoverflyProcess.WaitForExit(KILL_PROCESS_TIMEOUT);

            if (IsHoverflyProcessStillRunning())
                throw new TimeoutException("Timeout while waiting for hoverfly process to be closed.");
        }

        /// <summary>
        /// Closes all running hoverfly processes.
        /// </summary>
        /// <remarks>
        /// Sometimes there are allready running hoverfly process.
        /// By using this method all exitsing processes will be killed.
        /// </remarks>
        /// <exception cref="ApplicationException">
        /// Can be thrown if there are still running hoverfly processes after trying to closing them.
        /// </exception>
        public void CloseAllRunningHoverflyProcesses()
        {
            var processesToKill = Process.GetProcessesByName("hoverfly");

            foreach (var process in processesToKill)
            {
                process.Kill();
                process.WaitForExit(KILL_PROCESS_TIMEOUT);
            }

            if (Process.GetProcessesByName("hoverfly").Any())
                throw new ApplicationException("can't kill all existing running hoverfly processes.");
        }

        /// <summary>
        /// Imports hoverfly simulation data.
        /// </summary>
        /// <param name="source">The source of the simulation data to import.</param>
        public void ImportSimulation(ISimulationSource source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            _logger?.Info("Importing simulation data to Hoverfly.");

            var simulation = source.GetSimulation();

            if (simulation != null)
                _hoverflyClient.ImportSimulation(simulation);
        }

        /// <summary>
        /// Imports hoverfly simulation.
        /// </summary>
        /// <param name="simulation">The <see cref="Simulation"/> to import.</param>
        public void ImportSimulation(Simulation simulation)
        {
            if (simulation == null)
                throw new ArgumentNullException(nameof(simulation));

            _logger?.Info("Importing simulation data to Hoverfly.");

            _hoverflyClient.ImportSimulation(simulation);
        }

        /// <summary>
        /// Export hoverfly captured or simulated simulations.
        /// </summary>
        /// <param name="destinationSource">The destination source to where to export the simulation data.</param>
        public void ExportSimulation(ISimulationDestinationSource destinationSource)
        {
            if (destinationSource == null)
                throw new ArgumentNullException(nameof(destinationSource));

            _logger?.Info("Exporting simulation data from Hoverfly.");

            try
            {
                var simulation = _hoverflyClient.GetSimulation();
                destinationSource.SaveSimulation(simulation);
            }
            catch (Exception e)
            {
                throw new SimulationExportException($"Can't export simulation, reason: {e}", e);
            }
        }

        /// <summary>
        /// Gets the hoverfly captured or imported simulations.
        /// </summary>
        /// <returns>Retuns a <see cref="Simulation"/> that contains the simulation data.</returns>
        /// <remarks>Hoverfly simulation data.</remarks>
        public Simulation GetSimulation()
        {
            _logger?.Info("Get simulation data from Hoverfly.");

            return _hoverflyClient.GetSimulation();
        }

        /// <summary>
        /// Changes the hoverfly mode.
        /// </summary>
        /// <param name="mode">The <see cref="HoverflyMode"/> to change to.</param>
        public void ChangeMode(HoverflyMode mode)
        {
            _logger?.Info($"Changing mode to '{mode}'");
            _hoverflyClient.ChangeMode(mode);
        }

        /// <summary>
        /// Gets the hoverfly mode.
        /// </summary>
        /// <returns>Return the <see cref="HoverflyMode"/> the current hoverfly process uses.</returns>
        public HoverflyMode GetMode()
        {
            return _hoverflyClient.GetMode();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                Stop();

            _disposed = true;
        }

        private void SetProxySystemProperties()
        {
            if (_hoverflyMode == HoverflyMode.WebServer)
                return;

            //TODO: Temporary hack to accept all SSL
            ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => true;

            if (_hoverflyConfig.ProxyLocalhost)
            {
                WebRequest.DefaultWebProxy = new WebProxy($"http://localhost:{_hoverflyConfig.ProxyPort}", false);
            }
            else
            {
                WebRequest.DefaultWebProxy = new WebProxy(
                    $"http://localhost:{_hoverflyConfig.ProxyPort}",
                    true,
                    new[] { "local;*.local;169.254/16;*.169.254/16" });
            }
        }

        private void WaitForHoverflyToBecomeHealthy()
        {
            var timeoutDatTime = DateTime.Now.AddSeconds(BOOT_TIMEOUT_SECONDS);

            while (DateTime.Now < timeoutDatTime)
            {
                if (_hoverflyClient.IsHealthy())
                    return;

                Thread.Sleep(RETRY_BACKOFF_INTERVAL_MS);
            }

            throw new TimeoutException($"Hoverfly has not become healthy in '{BOOT_TIMEOUT_SECONDS}' seconds");
        }

        private bool IsHoverflyProcessStillRunning()
        {
            try
            {
                Process.GetProcessById(_hoverflyProcess.Id);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void StartHoverflyProcess()
        {
            VerifyPortNotInUse(_hoverflyConfig.ProxyPort);
            VerifyPortNotInUse(_hoverflyConfig.AdminPort);

            var hoverfilePath = string.IsNullOrWhiteSpace(_hoverflyConfig.HoverflyBasePath) ?
                               HOVERFLY_EXE :
                               Path.Combine(_hoverflyConfig.HoverflyBasePath, HOVERFLY_EXE);

            _logger?.Info("Start hoverfly.");

            if (TryStartHoverflyProcess(hoverfilePath))
                return;

            var hoverfileBasePath = GetHoverfileBasePath();

            var hoverflyPath = SearchForHoverflyFile(hoverfileBasePath);

            if (string.IsNullOrWhiteSpace(hoverflyPath))
                throw new FileNotFoundException($"Can't find the file '{HOVERFLY_EXE}' file in the '{hoverfileBasePath}' or in its sub-folders.");

            TryStartHoverflyProcess(hoverflyPath);
        }

        private string GetHoverfileBasePath()
        {
            return string.IsNullOrWhiteSpace(_hoverflyConfig.HoverflyBasePath) ?
                Environment.CurrentDirectory :
                _hoverflyConfig.HoverflyBasePath;
        }

        private bool TryStartHoverflyProcess(string hoverflyPath)
        {
            try
            {
                var processInfo = new ProcessStartInfo(hoverflyPath, GetHoverflyArgumentsBasedOnMode())
                {
                    WorkingDirectory = _hoverflyConfig.HoverflyBasePath,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                _hoverflyProcess = Process.Start(processInfo);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private string GetHoverflyArgumentsBasedOnMode()
        {
            var arguments = new StringBuilder();

            switch (_hoverflyMode)
            {
                case HoverflyMode.Capture:
                    arguments.Append(" -capture ");
                    break;
                case HoverflyMode.WebServer:
                    arguments.Append(" -webserver ");
                    break;
                case HoverflyMode.Simulate:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            arguments.Append(" -db memory ");
            arguments.Append($" -pp {_hoverflyConfig.ProxyPort} ");
            arguments.Append($" -ap {_hoverflyConfig.AdminPort} ");

            return arguments.ToString();
        }

        private static string SearchForHoverflyFile(string folder)
        {
            var result = Directory.GetFiles(folder, HOVERFLY_EXE, SearchOption.AllDirectories);
            return result.Any() ? result.First() : null;
        }

        private static void VerifyPortNotInUse(int port)
        {
            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            var tcpConnInfoArray = ipGlobalProperties.GetActiveTcpListeners();

            if (tcpConnInfoArray.Any(endpoint => endpoint.Port == port))
                throw new ConfigurationErrorsException($"Port '{port}' is already in use by other application, please use another one");
        }
    }
}