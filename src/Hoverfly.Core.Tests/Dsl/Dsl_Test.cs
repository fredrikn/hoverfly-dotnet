namespace Hoverfly.Core.Tests.Dsl
{
    using System.Linq;
    using System.Net.Http;

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

        [Fact]
        public void ShouldReturnCorrectNumberDelays_WhenGettingSimulation()
        {
            var service1 = HoverflyDsl.Service("www.my-test1.com")
                                .Get("/1").WithDelay(2000).WillReturn(ResponseCreators.Success("Hello World 1", "text/plain"))
                                .Get("/10").WithDelay(1000).WillReturn(ResponseCreators.Success("Hello World 10", "ptext/plain"));

            var service2 = HoverflyDsl.Service("www.my-test1.com").AddDelay("www.my-test1.com", 4000, HttpMethod.Post);

            var service3 = HoverflyDsl.Service("www.my-test1.com").Put("/test").WithDelay(100).WillReturn(ResponseCreators.Success("Hello World 3", "text/plain"));

            var dsl = new DslSimulationSource(service1, service2, service3);

            var simulation = dsl.GetSimulation();

            Assert.Equal(4, simulation.HoverflyData.GlobalActions.Delays.Count);
        }

        [Fact]
        public void ShouldReturnCorrectDelaySettings_WhenUsingGet()
        {
            var service1 =
                HoverflyDsl.Service("www.my-test1.com")
                    .Get("/a/b")
                    .WithDelay(2000)
                    .WillReturn(ResponseCreators.Success("Hello World 1", "text/plain"));

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
                HoverflyDsl.Service("www.my-test1.com")
                    .Put("/")
                    .WithDelay(2000)
                    .WillReturn(ResponseCreators.Success("Hello World 1", "text/plain"));

            var dsl = new DslSimulationSource(service1);

            var simulation = dsl.GetSimulation();

            Assert.Equal("www.my-test1.com/", simulation.HoverflyData.GlobalActions.Delays.First().UrlPattern);
            Assert.Equal(2000, simulation.HoverflyData.GlobalActions.Delays.First().Delay);
            Assert.Equal("PUT", simulation.HoverflyData.GlobalActions.Delays.First().HttpMethod);
        }

        [Fact]
        public void ShouldReturnCorrectDelaySettings_WhenUsingPost()
        {
            var service1 =
                HoverflyDsl.Service("www.my-test1.com/")
                    .Post("/1")
                    .WithDelay(2000)
                    .WillReturn(ResponseCreators.Success("Hello World 1", "text/plain"));

            var dsl = new DslSimulationSource(service1);

            var simulation = dsl.GetSimulation();

            Assert.Equal("www.my-test1.com/1", simulation.HoverflyData.GlobalActions.Delays.First().UrlPattern);
            Assert.Equal(2000, simulation.HoverflyData.GlobalActions.Delays.First().Delay);
            Assert.Equal("POST", simulation.HoverflyData.GlobalActions.Delays.First().HttpMethod);
        }
    }
}
