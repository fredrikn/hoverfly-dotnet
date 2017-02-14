namespace Hoverfly.Core.Resources
{
    using System;
    using System.Net.Http;
    using System.Text;

    using Logging;

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
        public void ImportSimulation(string simulationData)
        {
            var putSimulationTask = _hoverflyHttpClient.PutAsync(
                                                                 SIMULATION_PATH,
                                                                 new StringContent(simulationData, Encoding.UTF8, "application/json"));

            putSimulationTask.Wait();
            var response = putSimulationTask.Result;

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Can't add the simulation to Hoverfly, status code: '{response.StatusCode}', reason '{response.ReasonPhrase}'");
        }

        /// <summary>
        /// Cheks if hoverfly is running and is healty.
        /// </summary>
        /// <returns>Returns true if hoverfly is healthy.</returns>
        public bool IsHealthy()
        {
            try
            {
                var requestTask = _hoverflyHttpClient.GetAsync(HEALTH_CHECK_PATH);
                requestTask.Wait();

                using (var response = requestTask.Result)
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