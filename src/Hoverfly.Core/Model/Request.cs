using System.Collections.Generic;

namespace Hoverfly.Core.Model
{
    using Newtonsoft.Json;

    public class Request
    {
        public Request()
        {
        }

        public Request(
           IList<RequestFieldMatcher> path,
           IList<RequestFieldMatcher> method,
           IList<RequestFieldMatcher> destination,
           IList<RequestFieldMatcher> schema,
           IList<RequestFieldMatcher> body,
           Dictionary<string, IList<RequestFieldMatcher>> headers,
           Dictionary<string, IList<RequestFieldMatcher>> query)
        {
            Path = path;
            Method = method;
            Destination = destination;
            Scheme = schema;
            Body = body;
            Headers = headers;
            Query = query;
        }

        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public IList<RequestFieldMatcher> Path { get; set; }

        [JsonProperty("method", NullValueHandling = NullValueHandling.Ignore)]
        public IList<RequestFieldMatcher> Method { get; set; }

        [JsonProperty("destination", NullValueHandling = NullValueHandling.Ignore)]
        public IList<RequestFieldMatcher> Destination { get; set; }

        [JsonProperty("scheme", NullValueHandling = NullValueHandling.Ignore)]
        public IList<RequestFieldMatcher> Scheme { get; set; }

        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public IList<RequestFieldMatcher> Body { get; set; }

        [JsonProperty("headers", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, IList<RequestFieldMatcher>> Headers { get; set; }

        [JsonProperty("query", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, IList<RequestFieldMatcher>> Query { get; set; }
    }
}