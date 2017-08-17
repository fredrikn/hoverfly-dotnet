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
            string path,
            string method,
            string destination,
            string schema,
            string query,
            string body,
            Dictionary<string, IList<string>> headers)
        {
            Path = new FieldMatcher(path);
            Method = new FieldMatcher(method);
            Destination = new FieldMatcher(destination);
            Scheme = new FieldMatcher(schema);
            Query = new FieldMatcher(query);
            Body = new FieldMatcher(body);
            Headers = headers;
        }

        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public FieldMatcher Path { get; set; }

        [JsonProperty("method", NullValueHandling = NullValueHandling.Ignore)]
        public FieldMatcher Method { get; set; }

        [JsonProperty("destination", NullValueHandling = NullValueHandling.Ignore)]
        public FieldMatcher Destination { get; set; }

        [JsonProperty("scheme", NullValueHandling = NullValueHandling.Ignore)]
        public FieldMatcher Scheme { get; set; }

        [JsonProperty("query", NullValueHandling = NullValueHandling.Ignore)]
        public FieldMatcher Query { get; set; }

        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public FieldMatcher Body { get; set; }

        [JsonProperty("headers", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, IList<string>> Headers { get; set; }
    }
}