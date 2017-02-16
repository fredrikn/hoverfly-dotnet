using System.Collections.Generic;

namespace Hoverfly.Core.Model
{
    using Newtonsoft.Json;

    public class GlobalActions
    {
        [JsonProperty("delays")]
        public IList<DelaySettings> Delays { get; private set; }
    }
}
