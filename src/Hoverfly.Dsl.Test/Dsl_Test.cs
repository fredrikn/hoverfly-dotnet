namespace Hoverfly.Dsl.Test
{
    using Xunit;

    using static HoverflyDsl;
    using static ResponseCreators;

    public class Dsl_Test
    {
        [Fact]
        public void ShouldReturnCorrectNumberOfRequest_WhenGettingSimulation()
        {
            var service1 = Service("www.my-test1.com")
                                .Get("/1").WillReturn(Success("Hello World 1", "plain/text"))
                                .Get("/10").WillReturn(Success("Hello World 10", "plain/text"));

            var service2 = Service("www.my-test2.com").Get("/2").WillReturn(Success("Hello World 2", "plain/text"));
            var service3 = Service("www.my-test3.com").Get("/3").WillReturn(Success("Hello World 3", "plain/text"));

            var dsl = new DslSimulationSource(service1, service2, service3);

            var simulation = dsl.GetSimulation();

            Assert.Equal(4, simulation.HoverflyData.RequestResponsePair.Count);
        }
    }
}
