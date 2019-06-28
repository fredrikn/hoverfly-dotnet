namespace Hoverfly.Core.Model
{
    using Newtonsoft.Json;

    public class HoverflyMetaData
    {
        [JsonProperty("schemaVersion")]
        public string SchemaVersion { get; private set; } = "v5";

        [JsonProperty("hoverflyVersion", NullValueHandling = NullValueHandling.Ignore)]
        public string HoverflyVersion { get; private set; }

        [JsonProperty("timeExported", NullValueHandling = NullValueHandling.Ignore)]
        public string TimeExported { get; private set; }
    }
}
