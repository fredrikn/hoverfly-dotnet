namespace Hoverfly.Core.Resources
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Logging;

    /// <summary>
    /// The client that works against a hoverfly instance.
    /// </summary>
    public class HoverflyClient : IHoverflyClient
    {
        private const string HEALTH_CHECK_PATH = "/api/stats";
        private const string SIMULATION_PATH = "/api/v2/simulation";

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
        /// <param name="simulationData">The simulation as a hoverfly json simulation.</param>
        public void ImportSimulation(byte[] simulationData)
        {
            using (var response = Task.Run(() => _hoverflyHttpClient.PutAsync(SIMULATION_PATH, new StringContent(Encoding.UTF8.GetString(simulationData), Encoding.UTF8, "application/json")))
                                      .Result)
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Can't add the simulation to Hoverfly, status code: '{response.StatusCode}', reason '{response.ReasonPhrase}'");
            }
        }

        /// <summary>
        /// Gets the simulation recorded by hoverfly.
        /// </summary>
        public byte[] GetSimulation()
        {
            using (var response = Task.Run(() => _hoverflyHttpClient.GetAsync(SIMULATION_PATH)).Result)
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Can't get the simulation from Hoverfly, status code: '{response.StatusCode}', reason '{response.ReasonPhrase}'");

                return Task.Run(() => response.Content.ReadAsByteArrayAsync()).Result;
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