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
            var pairs = HoverflyDsl.Service("https://www.my-test.com").Get("/").WillReturn(ResponseBuilder.Response()).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("www.my-test.com", pair.Request.Destination.ExactMatch);
            Assert.Equal("https", pair.Request.Scheme.ExactMatch);
        }

        [Fact]
        public void ShouldCreateARequestWithHttpUrlScheme()
        {
            var pairs = HoverflyDsl.Service("http://www.my-test.com").Get("/").WillReturn(ResponseBuilder.Response()).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("www.my-test.com", pair.Request.Destination.ExactMatch);
            Assert.Equal("http", pair.Request.Scheme.ExactMatch);
        }

        [Fact]
        public void ShouldDefaultToHttpScheme_WhenCreatingRequestWithOutSchema()
        {
            var pairs = HoverflyDsl.Service("www.my-test.com").Get("/").WillReturn(ResponseBuilder.Response()).RequestResponsePairs;


            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("www.my-test.com", pair.Request.Destination.ExactMatch);
            Assert.Equal("http", pair.Request.Scheme.ExactMatch);
        }

        [Fact]
        public void ShouldCreateGetRequest()
        {
            var pairs = HoverflyDsl.Service("www.my-test.com").Get("/").WillReturn(ResponseBuilder.Response()).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("GET", pair.Request.Method.ExactMatch);
        }

        [Fact]
        public void ShouldCreatePostRequest()
        {
            var pairs = HoverflyDsl.Service("www.my-test.com").Post("/").WillReturn(ResponseBuilder.Response()).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("POST", pair.Request.Method.ExactMatch);
        }

        [Fact]
        public void ShouldCreatePutRequest()
        {
            var pairs = HoverflyDsl.Service("www.my-test.com").Put("/").WillReturn(ResponseBuilder.Response()).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("PUT", pair.Request.Method.ExactMatch);
        }

        [Fact]
        public void ShouldCreateDeleteRequest()
        {
            var pairs = HoverflyDsl.Service("www.my-test.com").Delete("/").WillReturn(ResponseBuilder.Response()).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("DELETE", pair.Request.Method.ExactMatch);
        }

        [Fact]
        public void ShouldCreateCorrectResponse()
        {
            var pairs = HoverflyDsl.Service("www.my-test.com").Get("/").WillReturn(ResponseCreators.Success("Hello World", "text/plain")).RequestResponsePairs;

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
                            .QueryParam("Id", 1,2,3)
                            .Header("Content-Type", "text/plain")
                            .Body("")
                            .WillReturn(ResponseCreators.Success("Hello World", "text/plain")).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("www.my-test.com", pair.Request.Destination.ExactMatch);
            Assert.Equal("/test", pair.Request.Path.ExactMatch);
            Assert.Equal("Id=1&Id=2&Id=3", pair.Request.Query.ExactMatch);
            Assert.Equal("text/plain", pair.Request.Headers["Content-Type"].First());
            Assert.Equal("", pair.Request.Body.ExactMatch);
        }
    }
}