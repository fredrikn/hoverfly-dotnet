using System.Linq;

namespace Hoverfly.Dsl.Test
{
    using Xunit;

    using static HoverflyDsl;
    using static ResponseBuilder;
    using static ResponseCreators;

    public class StubServiceBuilder_Test
    {
        [Fact]
        public void ShouldCreateARequestWithHttpsUrlScheme()
        {
            var pairs = Service("https://www.my-test.com").Get("/").WillReturn(Response()).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("www.my-test.com", pair.Request.Destination);
            Assert.Equal("https", pair.Request.Scheme);
        }

        [Fact]
        public void ShouldCreateARequestWithHttpUrlScheme()
        {
            var pairs = Service("http://www.my-test.com").Get("/").WillReturn(Response()).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("www.my-test.com", pair.Request.Destination);
            Assert.Equal("http", pair.Request.Scheme);
        }

        [Fact]
        public void ShouldDefaultToHttpScheme_WhenCreatingRequestWithOutSchema()
        {
            var pairs = Service("www.my-test.com").Get("/").WillReturn(Response()).RequestResponsePairs;


            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("www.my-test.com", pair.Request.Destination);
            Assert.Equal("http", pair.Request.Scheme);
        }

        [Fact]
        public void ShouldCreateGetRequest()
        {
            var pairs = Service("www.my-test.com").Get("/").WillReturn(Response()).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("GET", pair.Request.Method);
        }

        [Fact]
        public void ShouldCreatePostRequest()
        {
            var pairs = Service("www.my-test.com").Post("/").WillReturn(Response()).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("POST", pair.Request.Method);
        }

        [Fact]
        public void ShouldCreatePutRequest()
        {
            var pairs = Service("www.my-test.com").Put("/").WillReturn(Response()).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("PUT", pair.Request.Method);
        }

        [Fact]
        public void ShouldCreateDeleteRequest()
        {
            var pairs = Service("www.my-test.com").Delete("/").WillReturn(Response()).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("DELETE", pair.Request.Method);
        }

        [Fact]
        public void ShouldCreateCorrectResponse()
        {
            var pairs = Service("www.my-test.com").Get("/").WillReturn(Success("Hello World", "plain/text")).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal(200, pair.Response.Status);
            Assert.Equal("Hello World", pair.Response.Body);
            Assert.Equal("plain/text", pair.Response.Headers["Content-Type"].First());
            Assert.Equal(false, pair.Response.EncodedBody);
        }

        [Fact]
        public void ShouldCreateRequest()
        {
            var pairs = Service("www.my-test.com")
                            .Get("/test")
                            .QueryParam("Id", 1,2,3)
                            .Header("Content-Type", "plain/text")
                            .Body("")
                            .WillReturn(Success("Hello World", "plain/text")).RequestResponsePairs;

            Assert.Equal(1, pairs.Count);

            var pair = pairs.First();

            Assert.Equal("www.my-test.com", pair.Request.Destination);
            Assert.Equal("/test", pair.Request.Path);
            Assert.Equal("Id=1&Id=2&Id=3", pair.Request.Query);
            Assert.Equal("plain/text", pair.Request.Headers["Content-Type"].First());
            Assert.Equal("", pair.Request.Body);
        }
    }
}