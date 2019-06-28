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
        public void ShouldReturnCorrectSimulationData()
        {
            var fileSimulation = new FileSimulationSource("simulation_test.json");

            var simulation = fileSimulation.GetSimulation();

            var request = simulation.HoverflyData.RequestResponsePair.First().Request;
            var response = simulation.HoverflyData.RequestResponsePair.First().Response;

            Assert.Equal("GET", request.Method[0].Value);
            Assert.Equal(MatcherType.Exact, request.Method[0].Matcher);

            Assert.Equal("/key/value/one/two", request.Path[0].Value);
            Assert.Equal(MatcherType.Exact, request.Path[0].Matcher);

            Assert.Equal("echo.jsontest.com", request.Destination[0].Value);
            Assert.Equal(MatcherType.Exact, request.Destination[0].Matcher);

            Assert.Equal("http", request.Scheme[0].Value);
            Assert.Equal(MatcherType.Exact, request.Scheme[0].Matcher);

            Assert.Equal(200, response.Status);
            Assert.Equal("{\n   \"one\": \"two\",\n   \"key\": \"value\"\n}\n", response.Body);
        }

        [Fact]
        public void ShouldSaveTheSimulation()
        {
            var fileSimulation = new FileSimulationSource("simulation_save_test.json");

            var request = new Request
            {
                Scheme = new [] { new RequestFieldMatcher(MatcherType.Exact, "http") },
                Destination = new[] { new RequestFieldMatcher(MatcherType.Glob, "*.jsontest.com") },
                Method = new[]  { new RequestFieldMatcher("GET") },
                Path = new[] { new RequestFieldMatcher("/key/value/three/four") },
                Query = new Dictionary<string, IList<RequestFieldMatcher>> { { "name", new[] { new RequestFieldMatcher("test") } } },
                Headers = new Dictionary<string, IList<RequestFieldMatcher>> { { "Content-Type", new[] { new RequestFieldMatcher("application/json") } } }
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

            Assert.Equal(expectedRequest.Method[0].Matcher, request.Method[0].Matcher);
            Assert.Equal(expectedRequest.Path[0].Matcher, request.Path[0].Matcher);
            //Assert.Equal(expectedRequest.Query.Matcher, request.Query.Matcher);
            Assert.Equal(expectedRequest.Destination[0].Matcher, request.Destination[0].Matcher);
            Assert.Equal(expectedRequest.Scheme[0].Matcher, request.Scheme[0].Matcher);
            Assert.Equal(expectedRequest.Headers["Content-Type"][0].Value, request.Headers["Content-Type"][0].Value);
            Assert.Equal(expectedRequest.Headers["Content-Type"][0].Matcher, request.Headers["Content-Type"][0].Matcher);

            Assert.Equal(expectedresponse.Status, response.Status);
            Assert.Equal(expectedresponse.Body, response.Body);
            Assert.Equal(expectedresponse.EncodedBody, response.EncodedBody);
            Assert.Equal(expectedresponse.Headers["Content-Type"][0], response.Headers["Content-Type"][0]);

            // Clean up the test
            File.Delete("simulation_save_test.json");
        }
    }
}