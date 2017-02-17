using System.Collections.Generic;

namespace Hoverfly.Core.Model
{
    using Newtonsoft.Json;

    public class HoverflyData
    {
        public HoverflyData(
            IList<RequestResponsePair> requestResponsePairs,
            GlobalActions globalActions)
        {
            RequestResponsePair = requestResponsePairs;
            GlobalActions = globalActions;
        }

        [JsonProperty("pairs")]
        public IList<RequestResponsePair> RequestResponsePair { get; private set; }

        [JsonProperty("globalActions")]
        public GlobalActions GlobalActions { get; private set; }
    }
}