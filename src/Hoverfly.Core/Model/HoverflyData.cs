using System.Collections.Generic;

namespace Hoverfly.Core.Model
{
    using Newtonsoft.Json;

    public class HoverflyData
    {
        [JsonProperty("pairs")]
        public IList<RequestResponsePair> RequestResponsePair { get; private set; }

        [JsonProperty("globalActions")]
        public GlobalActions GlobalActions { get; private set; }
    }
}
