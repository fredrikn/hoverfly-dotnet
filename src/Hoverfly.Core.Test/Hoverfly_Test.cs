namespace Hoverfly.Core.Test
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Xunit;

    public class Hoverfly_Test
    {
        [Fact]
        public void ShouldExportSimulation()
        {
            var hoverfly = new Hoverfly(HoverflyMode.CAPTURE);

            hoverfly.Start();

            GetContentFrom("http://echo.jsontest.com/key/value/one/two");

            hoverfly.ExportSimulation("simulation.json");

            hoverfly.Stop();
        }

        [Fact]
        public void ShouldReturnCorrectSimulationDataResult_WhenHoverflyInWebserverModeImportingSimulationData()
        {
            var result = GetContentFrom("http://echo.jsontest.com/key/value/one/two");

            var hoverfly = new Hoverfly(HoverflyMode.WEBSERVER);

            hoverfly.Start();

            // Simulation_test.json holds a captured result from http://echo.jsontest.com/key/value/one/two
            hoverfly.ImportSimulation("simulation_test.json");

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

        private static string GetContentFrom(string url)
        {
            var response = Task.Run(() => new HttpClient().GetAsync(url)).Result;
            return Task.Run(() => response.Content.ReadAsStringAsync()).Result;
        }
    }
}