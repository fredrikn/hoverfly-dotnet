namespace Hoverfly.Core.Model
{
    using Newtonsoft.Json;

    public class Simulation
    {
        [JsonProperty("data")]
        public HoverflyData HoverflyData { get; private set; }

        [JsonProperty("meta")]
        public HoverflyMetaData HoverflyMetaData { get; private set; }
    }
}