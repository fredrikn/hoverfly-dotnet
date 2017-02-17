namespace Hoverfly.Core.Model
{
    using Newtonsoft.Json;

    public class Simulation
    {
        public Simulation(HoverflyData hoverflyData, HoverflyMetaData metaData)
        {
            HoverflyData = hoverflyData;
            HoverflyMetaData = metaData;
        }

        [JsonProperty("data")]
        public HoverflyData HoverflyData { get; private set; }

        [JsonProperty("meta")]
        public HoverflyMetaData HoverflyMetaData { get; private set; }
    }
}