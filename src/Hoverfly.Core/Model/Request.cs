using System.Collections.Generic;

namespace Hoverfly.Core.Model
{
    using Newtonsoft.Json;

    public class Request
    {
        [JsonProperty("requestType")]
        public string RequestType { get; private set; }

        [JsonProperty("path")]
        public string Path { get; private set; }

        [JsonProperty("method")]
        public string Method { get; private set; }

        [JsonProperty("destination")]
        public string Destination { get; private set; }

        [JsonProperty("scheme")]
        public string Scheme { get; private set; }

        [JsonProperty("query")]
        public string Query { get; private set; }

        [JsonProperty("body")]
        public string Body { get; private set; }

        [JsonProperty("headers")]
        public Dictionary<string, IList<string>> Headers { get; private set; }
    }
}
