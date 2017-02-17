namespace Hoverfly.Core.Test
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Model;

    using Resources;

    using Xunit;

    public class Hoverfly_Test
    {
        //TODO: The tests are now working against an external source, which can be down or be gone. Need to create a own Web server for the test.

        [Fact]
        public void ShouldExportSimulation()
        {
            var hoverfly = new Hoverfly(HoverflyMode.CAPTURE);

            hoverfly.Start();

            GetContentFrom("http://echo.jsontest.com/key/value/one/two");

            var destinatonSource = new FileSimulationSource("simulation.json");
            hoverfly.ExportSimulation(destinatonSource);

            hoverfly.Stop();
        }

        [Fact]
        public void ShouldReturnCorrectSimulationDataResult_WhenHoverflyInWebserverModeImportingSimulationData()
        {
            var result = GetContentFrom("http://echo.jsontest.com/key/value/one/two");

            var hoverfly = new Hoverfly(HoverflyMode.WEBSERVER);

            hoverfly.Start();

            // Simulation_test.json holds a captured result from http://echo.jsontest.com/key/value/one/two
            hoverfly.ImportSimulation(new FileSimulationSource("simulation_test.json"));

            var result2 = GetContentFrom("http://localhost:8500/key/value/one/two");

            hoverfly.Stop();

            Assert.Equal(result, result2);
        }

        [Fact]
        public void ShouldReturnCorrectSimulationDataResult_WhenHoverflyInSimulationMode()
        {
            var hoverfly = new Hoverfly(HoverflyMode.SIMULATE);

            hoverfly.Start();

            // The time.jsontest.com returns the current time and milliseconds from the server.
            var result = GetContentFrom("http://time.jsontest.com");

            Thread.Sleep(10);

            var result2 = GetContentFrom("http://time.jsontest.com");

            hoverfly.Stop();

            Assert.Equal(result, result2);
        }

        [Fact]
        public void ShouldReturnCorrectHoverflyMode()
        {
            var hoverfly = new Hoverfly(HoverflyMode.SIMULATE);

            hoverfly.Start();

            var mode = hoverfly.GetMode();

            hoverfly.Stop();

            Assert.Equal(HoverflyMode.SIMULATE, mode);
        }

        [Fact]
        public void ShouldReturnCorrectMode_WhenHoverflyModeIsChanged()
        {
            var hoverfly = new Hoverfly(HoverflyMode.SIMULATE);

            hoverfly.Start();

            hoverfly.ChangeMode(HoverflyMode.CAPTURE);

            var mode = hoverfly.GetMode();

            hoverfly.Stop();

            Assert.Equal(HoverflyMode.CAPTURE, mode);
        }

        [Fact]
        public void ShouldReturnSimluationFromHoverfly_WhenFileSimulationSourceIsUsed()
        {
            var hoverfly = new Hoverfly(HoverflyMode.WEBSERVER);

            hoverfly.Start();

            hoverfly.ImportSimulation(new FileSimulationSource("simulation_test.json"));

            var simulation = hoverfly.GetSimulation();

            hoverfly.Stop();

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
        public void ShouldReturnCorrectSimluationFromHoverfly_WhenImportingSimulation()
        {
            var hoverfly = new Hoverfly(HoverflyMode.SIMULATE);

            hoverfly.Start();

            var simulation = CreateTestSimulation();

            hoverfly.ImportSimulation(simulation);

            var expectedSimulation = hoverfly.GetSimulation();

            hoverfly.Stop();

            var expectedRequest = expectedSimulation.HoverflyData.RequestResponsePair.First().Request;
            var expectedResponse = expectedSimulation.HoverflyData.RequestResponsePair.First().Response;

            Assert.Equal(expectedRequest.Method, "GET");
            Assert.Equal(expectedRequest.Path, "/key/value/three/four");
            Assert.Equal(expectedRequest.Destination, "echo.jsontest.com");
            Assert.Equal(expectedRequest.Scheme, "http");

            Assert.Equal(expectedResponse.Status, 200);
            Assert.Equal(expectedResponse.Body, "{\n   \"three\": \"four\",\n   \"key\": \"value\"\n}\n");
        }

        [Fact]
        public void ShouldReturnCorrectRestultFromARequest_WhenImportingSimulation()
        {
            var hoverfly = new Hoverfly(HoverflyMode.WEBSERVER);

            hoverfly.Start();

            var simulation = CreateTestSimulation();

            hoverfly.ImportSimulation(simulation);

            var result = GetContentFrom("http://localhost:8500/key/value/three/four?name=test");

            hoverfly.Stop();

            Assert.Equal("{\n   \"three\": \"four\",\n   \"key\": \"value\"\n}\n",result);
        }


        private static Simulation CreateTestSimulation()
        {
            var request = new Request
                              {
                                  Scheme = "http",
                                  Destination = "echo.jsontest.com",
                                  Method = "GET",
                                  Path = "/key/value/three/four",
                                  Query = "name=test",
                                  Headers = new Dictionary<string, IList<string>> { { "Content-Type", new List<string> { "application/json" } } }
                              };

            var response = new Response
                               {
                                   Status = 200,
                                   Body = "{\n   \"three\": \"four\",\n   \"key\": \"value\"\n}\n",
                                   EncodedBody = false,
                                   Headers = new Dictionary<string, IList<string>> { { "Content-Type", new List<string> { "application/json" } } }
                               };

            var simulation =
                new Simulation(
                    new HoverflyData(
                        new List<RequestResponsePair> { new RequestResponsePair(request, response) }, null),
                    new HoverflyMetaData());
            return simulation;
        }

        private static string GetContentFrom(string url)
        {
            var response = Task.Run(() => new HttpClient().GetAsync(url)).Result;
            return Task.Run(() => response.Content.ReadAsStringAsync()).Result;
        }
    }
}