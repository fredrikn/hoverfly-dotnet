namespace Hoverfly.Core
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Threading;

    using Configuration;
    using Resources;
    using Logging;

    public class Hoverfly
    {
        private const int BOOT_TIMEOUT_SECONDS = 10;
        private const int RETRY_BACKOFF_INTERVAL_MS = 100;

        private readonly HoverflyConfig _hoverflyConfig;

        private readonly IHoverflyClient _hoverflyClient;

        private readonly ILoggerFactory _loggerFactory;

        private readonly ILog _logger;

        private readonly HoverflyMode _hoverflyMode;

        private Process _hoverflyProcess;

        public Hoverfly(HoverflyMode hoverflyMode) : this(null, hoverflyMode)
        {
        }

        public Hoverfly(
            HoverflyConfig config,
            HoverflyMode hoverflyMode) : this(config, hoverflyMode, null, null)
        {
        }

        public Hoverfly(
            HoverflyConfig config,
            HoverflyMode hoverflyMode,
            IHoverflyClient hoverflyClient) : this(config, hoverflyMode, hoverflyClient, null)
        {
        }

        public Hoverfly(
            HoverflyConfig config,
            HoverflyMode hoverflyMode,
            IHoverflyClient hoverflyClient,
            ILoggerFactory loggerFactory)
        {
            _hoverflyMode = hoverflyMode;

            _hoverflyConfig = config ?? HoverflyConfig.Config();
           
            _logger = loggerFactory?.Create(this.GetType().Name);

            _hoverflyClient = hoverflyClient ?? new HoverflyClient(
                                                         new Uri($"{_hoverflyConfig.RemoteHost}:{_hoverflyConfig.AdminPort}"),
                                                         _logger);
        }

        public void Start()
        {
            if (!_hoverflyConfig.IsRemoteInstance)
                StartHoverflyProcess();

            WaitForHoverflyToBecomeHealthy();
            SetProxySystemProperties();
        }

        public void Stop()
        {
            _logger?.Info("Destroying hoverfly process");
            _hoverflyProcess?.Kill();
        }

        public void ImportSimulation(ISimulationSource simulationSource)
        {
            if (simulationSource == null)
                throw new ArgumentNullException(nameof(simulationSource));

            _logger?.Info($"Importing simulation data to Hoverfly.");

            var simulationData = simulationSource.GetSimulation();

            if (simulationData == null)
                throw new SimulationEmptyException($"The hoverfly simulation data from source '{simulationSource.ResourcePath}' is empty.");

            _hoverflyClient.ImportSimulation(simulationData);
        }

        private void SetProxySystemProperties()
        {
            //TODO: When starting hoverfly webserver, no proxy is needed.

            WebRequest.DefaultWebProxy = new WebProxy(
                                                      $"{_hoverflyConfig.RemoteHost}:{_hoverflyConfig.ProxyPort}",
                                                      !_hoverflyConfig.ProxyLocalhost);
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

        private void StartHoverflyProcess()
        {
            var hoverflyPath = Path.Combine(_hoverflyConfig.HoverflyBasePath, "hoverfly.exe");

            if (!File.Exists(hoverflyPath))
                throw new FileNotFoundException($"Can't find the file hoverfly file at path '{hoverflyPath}'.");

            VerifyPortNotInUse(_hoverflyConfig.ProxyPort);
            VerifyPortNotInUse(_hoverflyConfig.AdminPort);

            _logger?.Info($"Start hoverfly from path '{hoverflyPath}'");

            var processInfo = new ProcessStartInfo(hoverflyPath, GetHoverflyArgumentsBasedOnMode())
                                  {
                                      WorkingDirectory = _hoverflyConfig.HoverflyBasePath,
                                      CreateNoWindow = false
                                  };

            _hoverflyProcess = Process.Start(processInfo);
        }

        private string GetHoverflyArgumentsBasedOnMode()
        {
            switch (_hoverflyMode)
            {
                case HoverflyMode.CAPTURE:
                    return " -capture";
                case HoverflyMode.WEBSERVER:
                    return " -webserver";
                case HoverflyMode.SIMULATE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
        }

        private static void VerifyPortNotInUse(int port)
        {
            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            var tcpConnInfoArray = ipGlobalProperties.GetActiveTcpListeners();

            if (tcpConnInfoArray.Any(endpoint => endpoint.Port == port))
            {
                throw new ConfigurationErrorsException($"Port '{port}' is already in use by other application, please use another one");
            }
        }
    }
}