namespace Hoverfly.Core.Tests
{
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using global::Hoverfly.Core.Dsl;
    using Model;
    using Resources;

    using Xunit;

    using static Core.Dsl.HoverflyDsl;
    using static Core.Dsl.ResponseCreators;


    public class HoverflyRunner_Test
    {
        [Fact]
        public void ShouldStartInSimulationMode()
        {
            using (var runner = HoverflyRunner.StartInSimulationMode(HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                Assert.Equal(HoverflyMode.Simulate, runner.GetHoverflyMode());
            }
        }

        [Fact]
        public void ShouldLoadSimulation_WhenStartInSimulationModeWithASimulationSource()
        {
            var fakeSource = new FileSimulationSource("simulation_test.json");
            using (var runner = HoverflyRunner.StartInSimulationMode(fakeSource, HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                var simulation = runner.GetSimulation();
                Assert.Equal("echo.jsontest.com", simulation.HoverflyData.RequestResponsePair.First().Request.Destination.ExactMatch);
                Assert.Equal(HoverflyMode.Simulate, runner.GetHoverflyMode());
            }
        }

        [Fact]
        public void ShouldLoadSimulation_WhenStartInSimulationModeWithASimulationSourceAsFile()
        {
            using (var runner = HoverflyRunner.StartInSimulationMode("simulation_test.json", HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                var simulation = runner.GetSimulation();
                Assert.Equal("echo.jsontest.com", simulation.HoverflyData.RequestResponsePair.First().Request.Destination.ExactMatch);
                Assert.Equal(HoverflyMode.Simulate, runner.GetHoverflyMode());
            }
        }

        [Fact]
        public void ShouldStartInCaptureMode()
        {
            var fakeDestination = new FakeSimulationDestinationSource();
            using (var runner = HoverflyRunner.StartInCaptureMode(fakeDestination, HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                Assert.Equal(HoverflyMode.Capture, runner.GetHoverflyMode());
            }
        }

        [Fact]
        public void ShouldExportSimulationOnStop_WhenInCaptureMode()
        {
            var fakeDestination = new FakeSimulationDestinationSource();

            using (var runner = HoverflyRunner.StartInCaptureMode(fakeDestination, HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
            }

            Assert.True(fakeDestination.WasSaved);
        }

        [Fact]
        public void ShouldReturnCorrectSimulation_WhenUsingSimulateWithDsl()
        {
            using (var runner = HoverflyRunner.StartInSimulationMode(HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                runner.Simulate(DslSimulationSource.Dsl(
                      Service("http://echo.jsontest.com")
                          .Get("/key/value/three/four")
                          .QueryParam("name", "test")
                          .WillReturn(
                              Success("Hello World!", "application/json"))));

                var result = GetContentFrom("http://echo.jsontest.com/key/value/three/four?name=test");
                Assert.Equal("Hello World!", result);
            }
        }

        [Fact]
        public void ShouldReturnCorrectSimulations_WhenUsingAnExistingSimulateAndWhenAddingOne()
        {
            using (var runner = HoverflyRunner.StartInSimulationMode("simulation_test.json", HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                runner.AddSimulation(DslSimulationSource.Dsl(
                      Service("http://echo.jsontest.com")
                          .Get("/key/value/six/seven")
                          .QueryParam("name", "test")
                          .WillReturn(
                              Success("Hello World!", "application/json"))));

                var simulation = runner.GetSimulation();
                Assert.Equal("echo.jsontest.com", simulation.HoverflyData.RequestResponsePair.First().Request.Destination.ExactMatch);
                Assert.Equal("/key/value/one/two", simulation.HoverflyData.RequestResponsePair.First().Request.Path.ExactMatch);

                Assert.Equal("echo.jsontest.com", simulation.HoverflyData.RequestResponsePair.Last().Request.Destination.ExactMatch);
                Assert.Equal("/key/value/six/seven", simulation.HoverflyData.RequestResponsePair.Last().Request.Path.ExactMatch);
            }
        }

        [Fact]
        public void ShouldGetSimulationResponse_WhenUsingSpyModeAndSimulationDslIsAreadyAdded()
        {
            using (var runner = HoverflyRunner.StartInSpyMode(HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                runner.AddSimulation(
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
        public void ShouldGetSimulationResponse_WhenUsingSpyModeAndSimulationDslIsAreadyAddedDuringStart()
        {
            var simulation = DslSimulationSource.Dsl(
                        Service("http://echo.jsontest.com")
                            .Get("/key/value/three/four")
                            .QueryParam("name", "test")
                            .WillReturn(
                                Success("MyData", "application/json")));

            using (var runner = HoverflyRunner.StartInSpyMode(simulation, HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                var result = GetContentFrom("http://echo.jsontest.com/key/value/three/four?name=test");

                Assert.Equal("MyData", result);
            }
        }

        [Fact]
        public void ShouldGetSimulationResponse_WhenUsingSpyModeAndSimulationFromFileIsAreadyAdded()
        {
            using (var runner = HoverflyRunner.StartInSpyMode("simulation_test.json", HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                var result = GetContentFrom("http://echo.jsontest.com/key/value/one/two");

                Assert.Equal("{\n   \"one\": \"two\",\n   \"key\": \"value\"\n}\n", result);
            }
        }

        [Fact]
        public void ShouldGetExternalResponse_WhenUsingSpyModeAndSimulationIsNotAdded()
        {
            using (var runner = HoverflyRunner.StartInSpyMode(HoverFlyTestConfig.GetHoverFlyConfigWIthBasePath()))
            {
                var result = GetContentFrom("http://echo.jsontest.com/key/value/one/two?name=testSpy");

                Assert.Equal("{\n   \"one\": \"two\",\n   \"key\": \"value\"\n}\n", result);
            }
        }


        private class FakeSimulationDestinationSource : ISimulationDestinationSource
        {
            public void SaveSimulation(Simulation simulation)
            {
                WasSaved = true;
            }

            public bool WasSaved { get; private set; } = false;
        }

        private class FakeSimulationSource : ISimulationSource
        {
            public Simulation GetSimulation()
            {
                WasCalled = true;
                return Simulation.Empty();
            }

            public bool WasCalled { get; private set; } = false;
        }

        private static string GetContentFrom(string url)
        {
            var response = Task.Run(() => new HttpClient().GetAsync(url)).Result;
            return Task.Run(() => response.Content.ReadAsStringAsync()).Result;
        }
    }
}