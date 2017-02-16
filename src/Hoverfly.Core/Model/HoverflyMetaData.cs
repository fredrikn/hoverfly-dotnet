using System;

namespace Hoverfly.Core.Model
{
    using Newtonsoft.Json;

    public class HoverflyMetaData
    {
        [JsonProperty("schemaVersion")]
        public string SchemaVersion { get; private set; } = "v1";

        [JsonProperty("hoverflyVersion")]
        public string HoverflyVersion { get; private set; }

        [JsonProperty("timeExported")]
        public DateTime TimeExported { get; private set; }
    }
}
