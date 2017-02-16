using System.Collections.Generic;

namespace Hoverfly.Core.Model
{
    using Newtonsoft.Json;

    public class Response
    {
        [JsonProperty("status")]
        public int Status { get; private set; }

        [JsonProperty("body")]
        public string Body { get; private set; }

        [JsonProperty("encodedBody")]
        public bool EncodedBody { get; private set; }

        [JsonProperty("headers")]
        public Dictionary<string, IList<string>> Headers { get; private set; }
    }
}
