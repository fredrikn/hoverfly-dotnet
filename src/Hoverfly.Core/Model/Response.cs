using System.Collections.Generic;

namespace Hoverfly.Core.Model
{
    using Newtonsoft.Json;

    public class Response
    {
        public Response()
        {
        }

        public Response(int status, string body, bool encodedBody, Dictionary<string, IList<string>> headers)
        {
            Status = status;
            Body = body;
            EncodedBody = encodedBody;
            Headers = headers;
        }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("encodedBody")]
        public bool EncodedBody { get; set; }

        [JsonProperty("headers")]
        public Dictionary<string, IList<string>> Headers { get; set; }
    }
}
