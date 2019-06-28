using System.Collections.Generic;

namespace Hoverfly.Core.Model
{
    using Newtonsoft.Json;

    public class Response
    {
        private Dictionary<string, string> _transitionsState;
        private List<string> _removesState;

        public Response()
        {
        }

        public Response(
            int status,
            string body,
            bool encodedBody,
            bool templated,
            Dictionary<string, IList<string>> headers,
            Dictionary<string, string> transitionsState,
            List<string> removesState)
        {
            Status = status;
            Body = body;
            Templated = templated;
            EncodedBody = encodedBody;
            Headers = headers;
            TransitionsState = transitionsState;
            RemovesState = removesState;
        }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("encodedBody")]
        public bool EncodedBody { get; set; }

        [JsonProperty("headers")]
        public Dictionary<string, IList<string>> Headers { get; set; }

        [JsonProperty("templated")]
        public bool Templated { get; set; }

        [JsonProperty("transitionsState")]
        public Dictionary<string, string> TransitionsState
        {
            get
            {
                if (_transitionsState == null)
                    _transitionsState = new Dictionary<string, string>();

                return _transitionsState;
            }
            set => _transitionsState = value;
        }

        [JsonProperty("removesState")]
        public List<string> RemovesState
        {
            get
            {
                if (_removesState == null)
                    _removesState = new List<string>();

                return _removesState;
            }
            set => _removesState = value; }
    }
}
