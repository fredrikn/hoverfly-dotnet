namespace Hoverfly.Core.Tests.Dsl
{
    using Core.Dsl;

    using Xunit;

    public class Dsl_Test
    {
        [Fact]
        public void ShouldReturnCorrectNumberOfRequest_WhenGettingSimulation()
        {
            var service1 = HoverflyDsl.Service("www.my-test1.com")
                                .Get("/1").WillReturn(ResponseCreators.Success("Hello World 1", "text/plain"))
                                .Get("/10").WillReturn(ResponseCreators.Success("Hello World 10", "ptext/plain"));

            var service2 = HoverflyDsl.Service("www.my-test2.com").Get("/2").WillReturn(ResponseCreators.Success("Hello World 2", "text/plain"));
            var service3 = HoverflyDsl.Service("www.my-test3.com").Get("/3").WillReturn(ResponseCreators.Success("Hello World 3", "text/plain"));

            var dsl = new DslSimulationSource(service1, service2, service3);

            var simulation = dsl.GetSimulation();

            Assert.Equal(4, simulation.HoverflyData.RequestResponsePair.Count);
        }
    }
}
