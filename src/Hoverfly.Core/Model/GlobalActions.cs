using System.Collections.Generic;

namespace Hoverfly.Core.Model
{
    using Newtonsoft.Json;

    public class GlobalActions
    {
        public GlobalActions(IList<DelaySettings> delays)
        {
            Delays = delays;
        }

        [JsonProperty("delays")]
        public IList<DelaySettings> Delays { get; private set; }
    }
}