namespace Hoverfly.Core.Model
{
    using System;

    using Newtonsoft.Json;

    public class RequestResponsePair
    {
        public RequestResponsePair(Request request, Response response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            Request = request;
            Response = response;
        }

        [JsonProperty("response")]
        public Response Response { get; private set; }

        [JsonProperty("request")]
        public Request Request { get; private set; }
    }
}
