namespace Hoverfly.Core.Model
{
    using Newtonsoft.Json;

    public class RequestResponsePair
    {
        [JsonProperty("response")]
        public Response Response { get; private set; }

        [JsonProperty("request")]
        public Request Request { get; private set; }
    }
}
