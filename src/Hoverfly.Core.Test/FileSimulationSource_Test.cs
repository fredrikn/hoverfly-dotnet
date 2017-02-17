namespace Hoverfly.Core.Test
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Model;

    using Resources;

    using Xunit;

    public class FileSimulationSource_Test
    {
        [Fact]
        public void ShouldReturnCorrectSimulationDate()
        {
            var fileSimulation = new FileSimulationSource("simulation_test.json");

            var simulation = fileSimulation.GetSimulation();

            var request = simulation.HoverflyData.RequestResponsePair.First().Request;
            var response = simulation.HoverflyData.RequestResponsePair.First().Response;

            Assert.Equal(request.Method, "GET");
            Assert.Equal(request.Path, "/key/value/one/two");
            Assert.Equal(request.Destination, "echo.jsontest.com");
            Assert.Equal(request.Scheme, "http");

            Assert.Equal(response.Status, 200);
            Assert.Equal(response.Body, "{\n   \"one\": \"two\",\n   \"key\": \"value\"\n}\n");
        }

        [Fact]
        public void ShouldSaveTheSimulation()
        {
            var fileSimulation = new FileSimulationSource("simulation_save_test.json");

            var requestHeaders = new Dictionary<string, IList<string>> { { "Content-Type", new List<string> { "application/json" } } };

            var request = new Request
                              {
                                  Scheme = "http",
                                  Destination = "echo.jsontest.com",
                                  Method = "GET",
                                  Path = "/key/value/three/four",
                                  Query = "?name=test",
                                  Headers = requestHeaders
                              };

            var responseHeaders = new Dictionary<string, IList<string>> { { "Content-Type", new List<string> { "application/json" } } };

            var response = new Response
                               {
                                   Status = 200,
                                   Body = "{\n   \"three\": \"four\",\n   \"key\": \"value\"\n}\n",
                                   EncodedBody = false,
                                   Headers = responseHeaders
                               };

            var simulation = new Simulation(
                new HoverflyData(
                    new List<RequestResponsePair>
                        {
                            new RequestResponsePair(request, response)
                        },
                    new GlobalActions(
                        new List<DelaySettings>
                            {
                                new DelaySettings(urlPattern: "echo.jsontest.com", delay: 2000)
                            })
                    ),
                    new HoverflyMetaData());

            fileSimulation.SaveSimulation(simulation);

            var fileSimulation2 = new FileSimulationSource("simulation_save_test.json");
            var simulation2 = fileSimulation2.GetSimulation();

            var expectedRequest = simulation2.HoverflyData.RequestResponsePair.First().Request;
            var expectedresponse = simulation2.HoverflyData.RequestResponsePair.First().Response;

            var expectedGlobalActionDelay = simulation2.HoverflyData.GlobalActions.Delays.First();

            Assert.Equal(expectedGlobalActionDelay.Delay, simulation.HoverflyData.GlobalActions.Delays.First().Delay);
            Assert.Equal(expectedGlobalActionDelay.UrlPattern, simulation.HoverflyData.GlobalActions.Delays.First().UrlPattern);

            Assert.Equal(expectedRequest.Method, request.Method);
            Assert.Equal(expectedRequest.Path, request.Path);
            Assert.Equal(expectedRequest.Query, request.Query);
            Assert.Equal(expectedRequest.Destination, request.Destination);
            Assert.Equal(expectedRequest.Scheme, request.Scheme);
            Assert.Equal(expectedRequest.Headers["Content-Type"][0], request.Headers["Content-Type"][0]);

            Assert.Equal(expectedresponse.Status, response.Status);
            Assert.Equal(expectedresponse.Body, response.Body);
            Assert.Equal(expectedresponse.EncodedBody, response.EncodedBody);
            Assert.Equal(expectedresponse.Headers["Content-Type"][0], response.Headers["Content-Type"][0]);

            // Clean up the test
            File.Delete("simulation_save_test.json");
        }
    }
}