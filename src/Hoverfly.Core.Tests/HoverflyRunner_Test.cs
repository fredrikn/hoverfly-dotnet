namespace Hoverfly.Core.Tests
{
    using System.Linq;

    using Model;
    using Resources;

    using Xunit;

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
            var fakeSource = new FakeSimulationSource();
            using (HoverflyRunner.StartInSimulationMode(fakeSource))
            {
                Assert.Equal(true, fakeSource.WasCalled);
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

            Assert.Equal(true, fakeDestination.WasCalled);
        }

        private class FakeSimulationDestinationSource : ISimulationDestinationSource
        {
            public void SaveSimulation(Simulation simulation)
            {
                WasCalled = true;
            }

            public bool WasCalled { get; private set; } = false;
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
    }
}