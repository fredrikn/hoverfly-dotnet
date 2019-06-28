namespace Hoverfly.Core.Tests.Dsl
{
    using System.Linq;

    using Core.Dsl;

    using Xunit;
    using static Core.Dsl.StubServiceBuilder;
    using static Core.Dsl.ResponseCreators;

    public class Dsl_Test
    {
        [Fact]
        public void ShouldReturnCorrectNumberOfRequest_WhenGettingSimulation()
        {
            var service1 = HoverflyDsl.Service("www.my-test1.com")
                                .Get("/1").WillReturn(Success("Hello World 1", "text/plain"))
                                .Get("/10").WillReturn(Success("Hello World 10", "ptext/plain"));

            var service2 = HoverflyDsl.Service("www.my-test2.com").Get("/2").WillReturn(ResponseCreators.Success("Hello World 2", "text/plain"));
            var service3 = HoverflyDsl.Service("www.my-test3.com").Get("/3").WillReturn(ResponseCreators.Success("Hello World 3", "text/plain"));

            var dsl = new DslSimulationSource(service1, service2, service3);

            var simulation = dsl.GetSimulation();

            Assert.Equal(4, simulation.HoverflyData.RequestResponsePair.Count);
        }

        [Fact]
        public void ShouldReturnCorrectNumberDelays_WhenGettingSimulation()
        {
            var service1 = HoverflyDsl.Service("www.my-test1.com")
                                        .Get("/1")
                                        .WillReturn(Success("Hello World 1", "text/plain").WithDelay(500))
                                        .Get("/10")
                                        .WillReturn(Success("Hello World 10", "ptext/plain"));

            var service2 = HoverflyDsl.Service("www.my-test1.com")
                                        .AndDelay(4000)
                                        .ForMethod(HttpMethod.GET);

            var service3 = HoverflyDsl.Service("www.my-test1.com")
                                        .Put("/test")
                                        .WillReturn(Success("Hello World 3", "text/plain"));

            var dsl = new DslSimulationSource(service1, service2, service3);

            var simulation = dsl.GetSimulation();

            Assert.Equal(2, simulation.HoverflyData.GlobalActions.Delays.Count);
        }

        [Fact]
        public void ShouldReturnCorrectDelaySettings_WhenUsingGet()
        {
            var service1 =
                HoverflyDsl.Service("www.my-test1.com")
                    .Get("/a/b")
                    .WillReturn(Success("Hello World 1", "text/plain")
                                .WithDelay(2000));

            var dsl = new DslSimulationSource(service1);

            var simulation = dsl.GetSimulation();

            Assert.Equal("www.my-test1.com/a/b", simulation.HoverflyData.GlobalActions.Delays.First().UrlPattern);
            Assert.Equal(2000, simulation.HoverflyData.GlobalActions.Delays.First().Delay);
            Assert.Equal("GET", simulation.HoverflyData.GlobalActions.Delays.First().HttpMethod);
        }

        [Fact]
        public void ShouldReturnCorrectDelaySettings_WhenUsingPut()
        {
            var service1 =
                HoverflyDsl.Service("www.my-test1.com").AndDelay(2000).ForMethod(HttpMethod.PUT)
                    .Put("/")
                    .WillReturn(ResponseCreators.Success("Hello World 1", "text/plain"));

            var dsl = new DslSimulationSource(service1);

            var simulation = dsl.GetSimulation();

            Assert.Equal("www.my-test1.com", simulation.HoverflyData.GlobalActions.Delays.First().UrlPattern);
            Assert.Equal(2000, simulation.HoverflyData.GlobalActions.Delays.First().Delay);
            Assert.Equal("PUT", simulation.HoverflyData.GlobalActions.Delays.First().HttpMethod);
        }

        [Fact]
        public void ShouldReturnCorrectDelaySettings_WhenUsingAnyMehtod()
        {
            var service1 =
                HoverflyDsl.Service("www.my-test1.com/").AndDelay(2000).ForAll()
                    .Post("/1")
                    .WillReturn(ResponseCreators.Success("Hello World 1", "text/plain"));

            var dsl = new DslSimulationSource(service1);

            var simulation = dsl.GetSimulation();

            Assert.Equal("www.my-test1.com/", simulation.HoverflyData.GlobalActions.Delays.First().UrlPattern);
            Assert.Equal(2000, simulation.HoverflyData.GlobalActions.Delays.First().Delay);
            Assert.Null(simulation.HoverflyData.GlobalActions.Delays.First().HttpMethod);
        }
    }
}
