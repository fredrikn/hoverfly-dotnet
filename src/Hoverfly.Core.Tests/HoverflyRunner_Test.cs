namespace Hoverfly.Core.Tests
{
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using global::Hoverfly.Core.Dsl;

    using Model;
    using Resources;

    using Xunit;

    using static Core.Dsl.HoverflyDsl;
    using static Core.Dsl.ResponseCreators;
    using static Core.Dsl.DslSimulationSource;

    public class HoverflyRunner_Test
    {
        [Fact]
        public void ShouldStartInSimulationMode()
        {
            using (var runner = HoverflyRunner.StartInSimulationMode())
            {
                Assert.Equal(HoverflyMode.Simulate, runner.GetHoverflyMode());                
            }
        }

        [Fact]
        public void ShouldLoadSimulation_WhenStartInSimulationModeWithASimulationSource()
        {
            var fakeSource = new FileSimulationSource("simulation_test.json");
            using (var runner = HoverflyRunner.StartInSimulationMode(fakeSource))
            {
                var simulation = runner.GetSimulation();
                Assert.Equal("echo.jsontest.com", simulation.HoverflyData.RequestResponsePair.First().Request.Destination);
                Assert.Equal(HoverflyMode.Simulate, runner.GetHoverflyMode());
            }
        }

        [Fact]
        public void ShouldLoadSimulation_WhenStartInSimulationModeWithASimulationSourceAsFile()
        {
            using (var runner = HoverflyRunner.StartInSimulationMode("simulation_test.json"))
            {
                var simulation = runner.GetSimulation();
                Assert.Equal("echo.jsontest.com", simulation.HoverflyData.RequestResponsePair.First().Request.Destination);
                Assert.Equal(HoverflyMode.Simulate, runner.GetHoverflyMode());
            }
        }

        [Fact]
        public void ShouldStartInCaptureMode()
        {
            var fakeDestination = new FakeSimulationDestinationSource();
            using (var runner = HoverflyRunner.StartInCaptureMode(fakeDestination))
            {
                Assert.Equal(HoverflyMode.Capture, runner.GetHoverflyMode());
            }
        }

        [Fact]
        public void ShouldExportSimulationOnStop_WhenInCaptureMode()
        {
            var fakeDestination = new FakeSimulationDestinationSource();
            var runner = HoverflyRunner.StartInCaptureMode(fakeDestination);

            runner.Dispose();

            Assert.Equal(true, fakeDestination.WasSaved);
        }

        [Fact]
        public void ShouldReturnCorrectSimulation_WhenUsingSimulateWithDsl()
        {
            using (var runner = HoverflyRunner.StartInSimulationMode())
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
            using (var runner = HoverflyRunner.StartInSimulationMode("simulation_test.json"))
            {
                runner.AddSimulation(DslSimulationSource.Dsl(
                      Service("http://echo.jsontest.com")
                          .Get("/key/value/six/seven")
                          .QueryParam("name", "test")
                          .WillReturn(
                              Success("Hello World!", "application/json"))));

                var simulation = runner.GetSimulation();
                Assert.Equal("echo.jsontest.com", simulation.HoverflyData.RequestResponsePair.First().Request.Destination);
                Assert.Equal("/key/value/one/two", simulation.HoverflyData.RequestResponsePair.First().Request.Path);

                Assert.Equal("echo.jsontest.com", simulation.HoverflyData.RequestResponsePair.Last().Request.Destination);
                Assert.Equal("/key/value/six/seven", simulation.HoverflyData.RequestResponsePair.Last().Request.Path);
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