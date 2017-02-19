namespace Hoverfly.Core.Model
{
    using System.Collections.Generic;

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

        public static Simulation Empty()
        {
            return new Simulation(
                new HoverflyData(
                    new List<RequestResponsePair>(),
                    new GlobalActions(
                        new List<DelaySettings>())),
                new HoverflyMetaData());
        }
    }
}