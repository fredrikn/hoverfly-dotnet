namespace Hoverfly.Core.Tests
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

            Assert.Equal("GET", request.Method.ExactMatch);
            Assert.Equal("/key/value/one/two", request.Path.ExactMatch);
            Assert.Equal("echo.jsontest.com", request.Destination.ExactMatch);
            Assert.Equal("http", request.Scheme.ExactMatch);

            Assert.Equal(200, response.Status);
            Assert.Equal("{\n   \"one\": \"two\",\n   \"key\": \"value\"\n}\n", response.Body);
        }

        [Fact]
        public void ShouldSaveTheSimulation()
        {
            var fileSimulation = new FileSimulationSource("simulation_save_test.json");

            var requestHeaders = new Dictionary<string, IList<string>> { { "Content-Type", new List<string> { "application/json" } } };

            var request = new Request
                              {
                                  Scheme = new FieldMatcher("http"),
                                  Destination = new FieldMatcher("echo.jsontest.com"),
                                  Method = new FieldMatcher("GET"),
                                  Path = new FieldMatcher("/key/value/three/four"),
                                  Query = new FieldMatcher("name=test"),
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

            Assert.Equal(expectedRequest.Method.ExactMatch, request.Method.ExactMatch);
            Assert.Equal(expectedRequest.Path.ExactMatch, request.Path.ExactMatch);
            Assert.Equal(expectedRequest.Query.ExactMatch, request.Query.ExactMatch);
            Assert.Equal(expectedRequest.Destination.ExactMatch, request.Destination.ExactMatch);
            Assert.Equal(expectedRequest.Scheme.ExactMatch, request.Scheme.ExactMatch);
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