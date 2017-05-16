namespace Hoverfly.Core.Model
{
    using Newtonsoft.Json;

    public class HoverflyMetaData
    {
        [JsonProperty("schemaVersion")]
        public string SchemaVersion { get; private set; } = "v2";
    }
}
