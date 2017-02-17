namespace Hoverfly.Core.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Configuration;

    using Model;

    using Resources;

    using Xunit;

    public class Hoverfly_Test
    {
        //TODO: The tests are now working against an external source, which can be down or be gone. Need to create a own Web server for the test.

        private readonly string _hoverflyPath = Path.Combine(Environment.CurrentDirectory,"..\\..\\..\\packages\\SpectoLabs.Hoverfly.0.10.1\\tools\\");

        [Fact]
        public void ShouldExportSimulation()
        {
            var config = HoverflyConfig.Config().SetHoverflyBasePath(_hoverflyPath);
            var hoverfly = new Hoverfly(HoverflyMode.Capture, config);

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

            var config = HoverflyConfig.Config().SetHoverflyBasePath(_hoverflyPath);
            var hoverfly = new Hoverfly(HoverflyMode.WebServer, config);

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
            var config = HoverflyConfig.Config().SetHoverflyBasePath(_hoverflyPath);
            var hoverfly = new Hoverfly(HoverflyMode.Simulate, config);

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
            var config = HoverflyConfig.Config().SetHoverflyBasePath(_hoverflyPath);
            var hoverfly = new Hoverfly(HoverflyMode.Simulate, config);

            hoverfly.Start();

            var mode = hoverfly.GetMode();

            hoverfly.Stop();

            Assert.Equal(HoverflyMode.Simulate, mode);
        }

        [Fact]
        public void ShouldReturnCorrectMode_WhenHoverflyModeIsChanged()
        {
            var config = HoverflyConfig.Config().SetHoverflyBasePath(_hoverflyPath);
            var hoverfly = new Hoverfly(HoverflyMode.Simulate, config);

            hoverfly.Start();

            hoverfly.ChangeMode(HoverflyMode.Capture);

            var mode = hoverfly.GetMode();

            hoverfly.Stop();

            Assert.Equal(HoverflyMode.Capture, mode);
        }

        [Fact]
        public void ShouldReturnSimluationFromHoverfly_WhenFileSimulationSourceIsUsed()
        {
            var config = HoverflyConfig.Config().SetHoverflyBasePath(_hoverflyPath);
            var hoverfly = new Hoverfly(HoverflyMode.WebServer, config);

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
            var config = HoverflyConfig.Config().SetHoverflyBasePath(_hoverflyPath);
            var hoverfly = new Hoverfly(HoverflyMode.Simulate, config);

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
        public void ShouldReturnCorrectRestultFromARequest_WhenImportingSimulationAndUsingWebServerMode()
        {
            var config = HoverflyConfig.Config().SetHoverflyBasePath(_hoverflyPath);
            var hoverfly = new Hoverfly(HoverflyMode.WebServer, config);

            hoverfly.Start();

            var simulation = CreateTestSimulation();

            hoverfly.ImportSimulation(simulation);

            var result = GetContentFrom("http://localhost:8500/key/value/three/four?name=test");

            hoverfly.Stop();

            Assert.Equal("{\n   \"three\": \"four\",\n   \"key\": \"value\"\n}\n",result);
        }

        [Fact]
        public void ShouldReturnCorrectRestultFromARequest_WhenImportingSimulationAndUsingSimulationMode()
        {
            var config = HoverflyConfig.Config().SetHoverflyBasePath(_hoverflyPath);
            var hoverfly = new Hoverfly(HoverflyMode.Simulate, config);

            hoverfly.Start();

            var simulation = CreateTestSimulation();

            hoverfly.ImportSimulation(simulation);

            var result = GetContentFrom("http://echo.jsontest.com/key/value/three/four?name=test");

            hoverfly.Stop();

            Assert.Equal("{\n   \"three\": \"four\",\n   \"key\": \"value\"\n}\n", result);
        }


        private static Simulation CreateTestSimulation()
        {
            return new Simulation(
                        new HoverflyData(
                            new List<RequestResponsePair> {
                                    new RequestResponsePair(
                                        new Request
                                        {
                                            Scheme = "http",
                                            Destination = "echo.jsontest.com",
                                            Method = "GET",
                                            Path = "/key/value/three/four",
                                            Query = "name=test",
                                            Headers = new Dictionary<string, IList<string>> { { "Content-Type", new List<string> { "application/json" } } }
                                        },
                                        new Response
                                        {
                                            Status = 200,
                                            Body = "{\n   \"three\": \"four\",\n   \"key\": \"value\"\n}\n",
                                            EncodedBody = false,
                                            Headers = new Dictionary<string, IList<string>> { { "Content-Type", new List<string> { "application/json" } } }
                                        })}, 
                            null),
                        new HoverflyMetaData());
        }

        private static string GetContentFrom(string url)
        {
            var response = Task.Run(() => new HttpClient().GetAsync(url)).Result;
            return Task.Run(() => response.Content.ReadAsStringAsync()).Result;
        }
    }
}