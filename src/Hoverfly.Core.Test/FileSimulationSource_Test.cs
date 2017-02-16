namespace Hoverfly.Core.Test
{
    using System.Linq;

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

        //[Fact]
        //public void ShouldReturnCorrectSimulationDate()
        //{
        //    var fileSimulation = new FileSimulationSource("simulation_test.json");

        //    var simulation = fileSimulation.GetSimulation();

        //    var request = simulation.HoverflyData.RequestResponsePair.First().Request;
        //    var response = simulation.HoverflyData.RequestResponsePair.First().Response;

        //    Assert.Equal(request.Method, "GET");
        //    Assert.Equal(request.Path, "/key/value/one/two");
        //    Assert.Equal(request.Destination, "echo.jsontest.com");
        //    Assert.Equal(request.Scheme, "http");

        //    Assert.Equal(response.Status, 200);
        //    Assert.Equal(response.Body, "{\n   \"one\": \"two\",\n   \"key\": \"value\"\n}\n");
        //}
    }
}
