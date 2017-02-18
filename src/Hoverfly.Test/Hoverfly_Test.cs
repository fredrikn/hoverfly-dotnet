namespace Hoverfly.Test
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Core;
    using Core.Configuration;

    using Xunit;

    using static Hoverfly.Core.Dsl.HoverflyDsl;
    using static Hoverfly.Core.Dsl.ResponseCreators;
    using static Hoverfly.Core.Dsl.DslSimulationSource;

    public class Hoverfly_Test
    {
        private readonly string _hoverflyPath = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\packages\\SpectoLabs.Hoverfly.0.10.1\\tools\\");


        [Fact]
        public void ShouldGetCorrectResponse_WhenUsingDsl()
        {
            var config = HoverflyConfig.Config().SetHoverflyBasePath(_hoverflyPath);

            var hoverfly = new Hoverfly(HoverflyMode.Simulate, config);

            hoverfly.Start();

            hoverfly.ImportSimulation(Dsl(
                Service("http://echo.jsontest.com")
                    .Get("/key/value/three/four")
                    .QueryParam("name", "test")
                    .WillReturn(Success("{\n   \"three\": \"four\",\n   \"key\": \"value\"\n}\n", "application/json"))));

            var result = GetContentFrom("http://echo.jsontest.com/key/value/three/four?name=test");

            hoverfly.Stop();

            Assert.Equal("{\n   \"three\": \"four\",\n   \"key\": \"value\"\n}\n", result);
        }

        private static string GetContentFrom(string url)
        {
            var response = Task.Run(() => new HttpClient().GetAsync(url)).Result;
            return Task.Run(() => response.Content.ReadAsStringAsync()).Result;
        }
    }
}
