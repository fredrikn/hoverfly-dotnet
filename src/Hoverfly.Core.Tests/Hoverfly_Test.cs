using System.Text;

namespace Hoverfly.Core.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Configuration;

    using Core.Dsl;

    using Model;
    using Resources;

    using Xunit;

    using static Core.Dsl.HoverflyDsl;
    using static Core.Dsl.ResponseCreators;
    using System;

    public class Hoverfly_Test
    {
        //TODO: The tests are now working against an external source, which can be down or be gone. Need to create local Web server for the test.

        [Fact]
        public void ShouldReturnCorrectConfiguredProxyPort()
        {
            var config = HoverflyConfig.Config().SetProxyPort(8600);
            var hoverfly = new Hoverfly(HoverflyMode.Simulate, config);

            Assert.Equal(8600, hoverfly.GetProxyPort());
        }


        [Fact]
        public void ShouldReturnCorrectConfiguredAdminPort()
        {
            var config = HoverflyConfig.Config().SetAdminPort(8880);
            var hoverfly = new Hoverfly(HoverflyMode.Simulate, config);

            Assert.Equal(8880, hoverfly.GetAdminPort());
        }


        [Fact]
        public void ShouldReturnSimulateMode_WhenHoverFlyIsSetToUseSimulateMode()
        {
            using (var hoverfly = new Hoverfly(HoverflyMode.Simulate, HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                hoverfly.Start();
                Assert.Equal(HoverflyMode.Simulate, hoverfly.GetMode());
            }
        }

        [Fact]
        public void ShouldReturnCaptureMode_WhenHoverFlyIsSetToUseCaptureMode()
        {
            using (var hoverfly = new Hoverfly(HoverflyMode.Capture, HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                hoverfly.Start();
                Assert.Equal(HoverflyMode.Capture, hoverfly.GetMode());
            }
        }


        [Fact]
        public void ShouldReturnSpyMode_WhenHoverFlyIsSetToUseSpyMode()
        {
            using (var hoverfly = new Hoverfly(HoverflyMode.Spy, HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                hoverfly.Start();
                Assert.Equal(HoverflyMode.Spy, hoverfly.GetMode());
            }
        }

        [Fact]
        public void ShouldExportSimulation()
        {
            using (var hoverfly = new Hoverfly(HoverflyMode.Capture, HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                hoverfly.Start();

                GetContentFrom("http://echo.jsontest.com/key/value/one/two");

                //http://localhost:8888/api/v2/simulation
                var destinatonSource = new FileSimulationSource("simulation.json");
                hoverfly.ExportSimulation(destinatonSource);

                hoverfly.Stop();
            }
        }

        [Fact]
        public void ShouldReturnCorrectSimulationDataResult_WhenHoverflyInSimulationMode()
        {
            using (var hoverfly = new Hoverfly(HoverflyMode.Simulate, HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {

                hoverfly.Start();

                // The time.jsontest.com returns the current time and milliseconds from the server.
                var result = GetContentFrom("http://time.jsontest.com");

                Thread.Sleep(10);

                var result2 = GetContentFrom("http://time.jsontest.com");

                hoverfly.Stop();

                Assert.Equal(result, result2);
            }
        }

        [Fact]
        public void ShouldReturnCorrectHoverflyMode()
        {
            using (var hoverfly = new Hoverfly(HoverflyMode.Simulate, HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {

                hoverfly.Start();

                var mode = hoverfly.GetMode();

                hoverfly.Stop();

                Assert.Equal(HoverflyMode.Simulate, mode);
            }
        }

        [Fact]
        public void ShouldReturnCorrectMode_WhenHoverflyModeIsChanged()
        {
            using (var hoverfly = new Hoverfly(HoverflyMode.Simulate, HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {

                hoverfly.Start();

                hoverfly.ChangeMode(HoverflyMode.Capture);

                var mode = hoverfly.GetMode();

                hoverfly.Stop();

                Assert.Equal(HoverflyMode.Capture, mode);
            }
        }

        [Fact]
        public void ShouldReturnCorrectSimluationFromHoverfly_WhenImportingSimulation()
        {
            using (var hoverfly = new Hoverfly(HoverflyMode.Simulate, HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {

                hoverfly.Start();

                var simulation = CreateTestSimulation();

                hoverfly.ImportSimulation(simulation);

                var expectedSimulation = hoverfly.GetSimulation();

                hoverfly.Stop();

                var expectedRequest = expectedSimulation.HoverflyData.RequestResponsePair.First().Request;
                var expectedResponse = expectedSimulation.HoverflyData.RequestResponsePair.First().Response;

                Assert.Equal("GET", expectedRequest.Method.ExactMatch);
                Assert.Equal("/key/value/three/four", expectedRequest.Path.ExactMatch);
                Assert.Equal("echo.jsontest.com", expectedRequest.Destination.ExactMatch);
                Assert.Equal("http", expectedRequest.Scheme.ExactMatch);

                Assert.Equal(200, expectedResponse.Status);
                Assert.Equal("{\n   \"three\": \"four\",\n   \"key\": \"value\"\n}\n", expectedResponse.Body);
            }
        }

        [Fact]
        public async Task ShouldReturnCorrectRestultFromAPutRequest_WhenImportingSimulationAndUsingSimulationMode()
        {
            using (var hoverfly = new Hoverfly(HoverflyMode.Simulate, HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                hoverfly.Start();

                hoverfly.ImportSimulation(new FileSimulationSource("simulation_test2.json"));

                var httpClient = new HttpClient();

                var content = new StringContent("{\"items\":[{\"sku\":\"6948017\",\"quantity\":2}]}", Encoding.UTF8, "application/json");

                var result = await httpClient.PutAsync("https://echo.jsontest.com/cart", content);

                var contentRestult = await result.Content.ReadAsStringAsync();

                hoverfly.Stop();

                Assert.Equal("{\n   \"one\": \"two\",\n   \"key\": \"value\"\n}\n", contentRestult);
            }
        }

        [Fact]
        public void ShouldReturnCorrectRestultFromARequest_WhenImportingSimulationAndUsingSimulationMode()
        {
            using (var hoverfly = new Hoverfly(HoverflyMode.Simulate, HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                hoverfly.Start();

                var simulation = CreateTestSimulation();

                hoverfly.ImportSimulation(simulation);

                var result = GetContentFrom("http://echo.jsontest.com/key/value/three/four?name=test");

                hoverfly.Stop();

                Assert.Equal("{\n   \"three\": \"four\",\n   \"key\": \"value\"\n}\n", result);
            }
        }

        [Fact]
        public void ShouldGetCorrectResponse_WhenUsingDsl()
        {
            using (var hoverfly = new Hoverfly(HoverflyMode.Simulate, HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                hoverfly.Start();

                hoverfly.ImportSimulation(
                    DslSimulationSource.Dsl(
                        Service("http://echo.jsontest.com")
                            .Get("/key/value/three/four")
                            .QueryParam("name", "test")
                            .WillReturn(
                                Success("{\n   \"three\": \"four\",\n   \"key\": \"value\"\n}\n", "application/json"))));

                var result = GetContentFrom("http://echo.jsontest.com/key/value/three/four?name=test");

                Assert.Equal("{\n   \"three\": \"four\",\n   \"key\": \"value\"\n}\n", result);
            }
        }

        [Fact]
        public void ShouldReturnCorrectSimulations_WhenUsingAnExistingSimulateAndWhenAddingOne()
        {
            using (var hoverfly = new Hoverfly(HoverflyMode.Simulate, HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                hoverfly.Start();

                hoverfly.ImportSimulation(new FileSimulationSource("simulation_test.json"));

                hoverfly.AddSimulation(DslSimulationSource.Dsl(
                      Service("http://echo.jsontest.com")
                          .Get("/key/value/six/seven")
                          .QueryParam("name", "test")
                          .WillReturn(
                              Success("Hello World!", "application/json"))));

                var simulation = hoverfly.GetSimulation();

                hoverfly.Stop();

                Assert.Equal("echo.jsontest.com", simulation.HoverflyData.RequestResponsePair.First().Request.Destination.ExactMatch);
                Assert.Equal("/key/value/one/two", simulation.HoverflyData.RequestResponsePair.First().Request.Path.ExactMatch);

                Assert.Equal("echo.jsontest.com", simulation.HoverflyData.RequestResponsePair.Last().Request.Destination.ExactMatch);
                Assert.Equal("/key/value/six/seven", simulation.HoverflyData.RequestResponsePair.Last().Request.Path.ExactMatch);
            }
        }

        [Fact]
        public void ShouldGetSimulationResponse_WhenUsingSpyModeAndSimulationIsAreadyAdded()
        {
            using (var hoverfly = new Hoverfly(HoverflyMode.Spy, HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                hoverfly.Start();

                hoverfly.ImportSimulation(
                    DslSimulationSource.Dsl(
                        Service("http://echo.jsontest.com")
                            .Get("/key/value/three/four")
                            .QueryParam("name", "test")
                            .WillReturn(
                                Success("MyData", "application/json"))));

                var result = GetContentFrom("http://echo.jsontest.com/key/value/three/four?name=test");

                Assert.Equal("MyData", result);
            }
        }

        [Fact]
        public void ShouldGetExternalResponse_WhenUsingSpyModeAndSimulationIsNotAdded()
        {
            using (var hoverfly = new Hoverfly(HoverflyMode.Spy, HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                hoverfly.Start();

                var result = GetContentFrom("http://echo.jsontest.com/key/value/one/two?name=testSpy");

                Assert.Equal("{\n   \"one\": \"two\",\n   \"key\": \"value\"\n}\n", result);
            }
        }

        [Fact]
        public void ShouldGetHeaderInTheSimulation_WhenCapturingSpecificHeader()
        {
            var config = HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath();

            config.SetCaptureHeaders("My-Header");

            using (var hoverfly = new Hoverfly(HoverflyMode.Capture, config))
            {
                hoverfly.Start();

                var header = new Dictionary<string, string> { { "My-Header", "Value" } };

                var result = GetContentFrom(
                    "http://echo.jsontest.com/key/value/three/four?name=test",
                    header);

                var simulation = hoverfly.GetSimulation();
                var capturedHeader = simulation.HoverflyData.RequestResponsePair.First().Request.Headers;

                Assert.NotNull(capturedHeader);
                Assert.Equal("My-Header", capturedHeader.First().Key);
            }
        }

        [Fact]
        public void ShouldNotCaptureNoneCaptureHeader_WhenCapturingSpecificHeader()
        {
            var config = HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath();

            config.SetCaptureHeaders("My-Header");

            using (var hoverfly = new Hoverfly(HoverflyMode.Capture, config))
            {
                hoverfly.Start();

                var header = new Dictionary<string, string> { { "MY-HEADER-NOT-CAPTURED", "Value" } };

                var result = GetContentFrom(
                    "http://echo.jsontest.com/key/value/three/four?name=test",
                    header);

                var simulation = hoverfly.GetSimulation();
                var capturedHeader = simulation.HoverflyData.RequestResponsePair.First().Request.Headers;

                Assert.Null(capturedHeader);
            }
        }

        [Fact]
        public void ShouldGetHeaderInTheSimulation_WhenCapturingAllHeaders()
        {
            var config = HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath();

            config.CaptureAllHeaders();

            using (var hoverfly = new Hoverfly(HoverflyMode.Capture, config))
            {
                hoverfly.Start();

                var header = new Dictionary<string, string> { { "My-Header", "Value" } };

                var result = GetContentFrom(
                    "http://echo.jsontest.com/key/value/three/four?name=test",
                    header);

                var simulation = hoverfly.GetSimulation();
                var capturedHeader = simulation.HoverflyData.RequestResponsePair.First().Request.Headers;

                Assert.NotNull(capturedHeader);
            }
        }

        [Fact]
        public void ShouldUseRemoteHovervyInstance()
        {
            var config = HoverflyConfig.Config().SetHoverflyBasePath(HoverFlyTestConfig.PackagePath);
            using (var hoverfly = new Hoverfly(HoverflyMode.Simulate, config))
            {
                hoverfly.Start();

                hoverfly.ImportSimulation(
                    DslSimulationSource.Dsl(
                        Service("http://echo.jsontest.com")
                            .Get("/key/value/three/four")
                            .QueryParam("name", "test")
                            .WillReturn(
                                Success("{\n   \"three\": \"four\",\n   \"key\": \"value\"\n}\n", "application/json"))));

                var simulation = hoverfly.GetSimulation();
                
                var config2 = HoverflyConfig.Config().UseRemoteInstance(config.RemoteHost, config.ProxyPort, config.AdminPort);
                using (var reuseHoverfly = new Hoverfly(config: config2))
                {
                    var simulation2 = reuseHoverfly.GetSimulation();

                    Assert.Equal(hoverfly.GetAdminPort(), reuseHoverfly.GetAdminPort());
                    Assert.Equal(hoverfly.GetProxyPort(), reuseHoverfly.GetProxyPort());
                    Assert.Equal(simulation.HoverflyData.RequestResponsePair.First().Response.Body, simulation2.HoverflyData.RequestResponsePair.First().Response.Body);
                }
            }
        }

        [Fact]
        public void ShouldBeDelay_WhenAddingADelaryToRequestWithDsl()
        {
            using (var hoverfly = new Hoverfly(HoverflyMode.Simulate, HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                hoverfly.Start();

                hoverfly.ImportSimulation(
                    DslSimulationSource.Dsl(
                        Service("http://echo.jsontest.com")
                            .Get("/key/value/three/four")
                            .WithDelay(2000)
                            .WillReturn(Success("Test", "application/json"))));

                var stopWatch = Stopwatch.StartNew();
                GetContentFrom("http://echo.jsontest.com/key/value/three/four");
                stopWatch.Stop();

                Assert.True(stopWatch.Elapsed.TotalMilliseconds >= 2000);
            }
        }

        private static Simulation CreateTestSimulation()
        {
            return new Simulation(
                        new HoverflyData(
                            new List<RequestResponsePair> {
                                    new RequestResponsePair(
                                        new Request
                                        {
                                            Scheme = new FieldMatcher("http"),
                                            Destination = new FieldMatcher("echo.jsontest.com"),
                                            Method = new FieldMatcher("GET"),
                                            Path = new FieldMatcher("/key/value/three/four"),
                                            Query = new FieldMatcher("name=test"),
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

        private static string GetContentFrom(string url, Dictionary<string,string> headers = null)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };

            if (headers != null && headers.Any())
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            var response = Task.Run(() => client.SendAsync(request)).Result;
            return Task.Run(() => response.Content.ReadAsStringAsync()).Result;
        }
    }
}