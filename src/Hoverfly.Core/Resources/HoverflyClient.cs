namespace Hoverfly.Core.Resources
{
    using System;
    using System.ComponentModel;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Logging;
    using Model;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// The client that works against a hoverfly instance.
    /// </summary>
    public class HoverflyClient : IHoverflyClient
    {
        private const string HEALTH_CHECK_PATH = "/api/health";
        private const string SIMULATION_PATH = "/api/v2/simulation";
        private const string HOVERFLY_MODE_PATH = "/api/v2/hoverfly/mode";

        private readonly HttpClient _hoverflyHttpClient;

        private readonly ILog _logger;

        public HoverflyClient(Uri hoverflyAdminBaseUri, ILog logger)
        {
            if (hoverflyAdminBaseUri == null)
                throw new ArgumentNullException(nameof(hoverflyAdminBaseUri));

            _hoverflyHttpClient = new HttpClient { BaseAddress = hoverflyAdminBaseUri };
            _logger = logger;
        }

        /// <summary>
        /// Imports simulation to hoverfly.
        /// </summary>
        /// <param name="simulation">The <see cref="Simulation"/> to import.</param>
        public void ImportSimulation(Simulation simulation)
        {
            var body = new StringContent(JsonConvert.SerializeObject(simulation), Encoding.UTF8, "application/json");

            using (var response = Task.Run(() => _hoverflyHttpClient.PutAsync(SIMULATION_PATH, body))
                                      .Result)
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Can't add the simulation to Hoverfly, status code: '{response.StatusCode}', reason '{response.ReasonPhrase}'");
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

            using (var response = Task.Run(() => _hoverflyHttpClient.GetAsync(SIMULATION_PATH)).Result)
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Can't get the simulation from Hoverfly, status code: '{response.StatusCode}', reason '{response.ReasonPhrase}'");

                var result = Encoding.UTF8.GetString(Task.Run(() => response.Content.ReadAsByteArrayAsync()).Result);

                return JsonConvert.DeserializeObject<Simulation>(result);
            }
        }

        /// <summary>
        /// Changes the hoverfly mode.
        /// </summary>
        /// <param name="mode">The <see cref="HoverflyMode"/> to change to.</param>
        public void SetMode(HoverflyMode mode)
        {
            SetMode(new ModeCommand(mode));
        }

        /// <summary>
        /// Changes the hoverfly mode.
        /// </summary>
        /// <param name="mode">The <see cref="ModeCommand"/> to change to.</param>
        public void SetMode(ModeCommand modeCommand)
        {
            var content = JsonConvert.SerializeObject(modeCommand, new StringEnumConverter(true));

            var modeBody = new StringContent(content, Encoding.UTF8, "application/json");

            using (var response = Task.Run(() => _hoverflyHttpClient.PutAsync(HOVERFLY_MODE_PATH, modeBody)).Result)
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Can't change the mode to {modeCommand.Mode}, status code: '{response.StatusCode}', reason '{response.ReasonPhrase}'");
            }
        }

        /// <summary>
        /// Gets the hoverfly mode.
        /// </summary>
        /// <returns>Return the <see cref="HoverflyMode"/> the current hoverfly process uses.</returns>
        public HoverflyMode GetMode()
        {
            using (var response = Task.Run(() => _hoverflyHttpClient.GetAsync(HOVERFLY_MODE_PATH)).Result)
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Can't get the mode from hoverfly, status code: '{response.StatusCode}', reason '{response.ReasonPhrase}'");

                dynamic result = JObject.Parse(Task.Run(() => response.Content.ReadAsStringAsync()).Result);

                HoverflyMode mode;

                if (Enum.TryParse((string)result.mode, true, out mode))
                    return mode;

                throw new InvalidEnumArgumentException($"Can't parse mode {mode} to any of the enum value in the HoverflyMode enum.");

            }
        }

        /// <summary>
        /// Cheks if hoverfly is running and is healty.
        /// </summary>
        /// <returns>Returns true if hoverfly is healthy.</returns>
        public bool IsHealthy()
        {
            try
            {
                using (var response = Task.Run(() => _hoverflyHttpClient.GetAsync(HEALTH_CHECK_PATH)).Result)
                {
                    _logger?.Info($"Hoverfly health check status code is: {response.StatusCode}");

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception e)
            {
                _logger?.Info($"Not yet healthy '{e}'");
            }

            return false;
        }
    }
}