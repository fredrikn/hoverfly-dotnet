using System.Collections.Generic;

namespace Hoverfly.Core.Model
{
    using Newtonsoft.Json;

    public class Request
    {
        [JsonProperty("requestType")]
        public string RequestType { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("destination")]
        public string Destination { get; set; }

        [JsonProperty("scheme")]
        public string Scheme { get; set; }

        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("headers")]
        public Dictionary<string, IList<string>> Headers { get; set; }
    }
}
