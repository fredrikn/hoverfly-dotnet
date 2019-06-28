namespace Hoverfly.Core.Tests.Dsl
{
    using System.Linq;

    using Core.Dsl;

    using Xunit;

    public class StubServiceBuilder_Test
    {
        [Fact]
        public void ShouldCreateARequestWithHttpsUrlScheme()
        {
            var pairs = HoverflyDsl.Service("https://www.my-test.com")
                                        .Get("/")
                                        .WillReturn(ResponseBuilder.Response()).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("www.my-test.com", pair.Request.Destination[0].Value);
            Assert.Equal("https", pair.Request.Scheme[0].Value);
        }

        [Fact]
        public void ShouldCreateARequestWithHttpUrlScheme()
        {
            var pairs = HoverflyDsl.Service("http://www.my-test.com")
                                        .Get("/")
                                        .WillReturn(ResponseBuilder.Response()).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("www.my-test.com", pair.Request.Destination[0].Value);
            Assert.Equal("http", pair.Request.Scheme[0].Value);
        }

        [Fact]
        public void ShouldSetHttpSchemeToNull_WhenCreatingRequestWithOutSchema()
        {
            var pairs = HoverflyDsl.Service("www.my-test.com")
                                        .Get("/")
                                        .WillReturn(ResponseBuilder.Response()).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("www.my-test.com", pair.Request.Destination[0].Value);
            Assert.Null(pair.Request.Scheme);
        }

        [Fact]
        public void ShouldCreateGetRequest()
        {
            var pairs = HoverflyDsl.Service("www.my-test.com")
                                        .Get("/")
                                        .WillReturn(ResponseBuilder.Response()).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("GET", pair.Request.Method[0].Value);
        }

        [Fact]
        public void ShouldCreatePostRequest()
        {
            var pairs = HoverflyDsl.Service("www.my-test.com")
                                        .Post("/")
                                        .WillReturn(ResponseBuilder.Response()).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("POST", pair.Request.Method[0].Value);
        }

        [Fact]
        public void ShouldCreatePutRequest()
        {
            var pairs = HoverflyDsl.Service("www.my-test.com")
                                        .Put("/")
                                        .WillReturn(ResponseBuilder.Response()).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("PUT", pair.Request.Method[0].Value);
        }

        [Fact]
        public void ShouldCreateDeleteRequest()
        {
            var pairs = HoverflyDsl.Service("www.my-test.com")
                                        .Delete("/")
                                        .WillReturn(ResponseBuilder.Response()).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("DELETE", pair.Request.Method[0].Value);
        }

        [Fact]
        public void ShouldCreateCorrectResponse()
        {
            var pairs = HoverflyDsl.Service("www.my-test.com")
                                    .Get("/")
                                    .WillReturn(ResponseCreators.Success("Hello World", "text/plain")).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal(200, pair.Response.Status);
            Assert.Equal("Hello World", pair.Response.Body);
            Assert.Equal("text/plain", pair.Response.Headers["Content-Type"].First());
            Assert.False(pair.Response.EncodedBody);
        }

        [Fact]
        public void ShouldCreateRequest()
        {
            var pairs = HoverflyDsl.Service("www.my-test.com")
                            .Get("/test")
                            .QueryParam("Id", 1, 2, 3)
                            .Header("Content-Type", "text/plain")
                            .Body("")
                            .WillReturn(ResponseCreators.Success("Hello World", "text/plain")).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("www.my-test.com", pair.Request.Destination[0].Value);
            Assert.Equal("/test", pair.Request.Path[0].Value);
            Assert.Equal("1;2;3", pair.Request.Query["Id"][0].Value);
            Assert.Equal("text/plain", pair.Request.Headers["Content-Type"][0].Value);
            Assert.Equal("", pair.Request.Body[0].Value);
        }
    }
}