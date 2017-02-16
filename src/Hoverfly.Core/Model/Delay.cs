namespace Hoverfly.Core.Model
{
    using Newtonsoft.Json;

    public class DelaySettings
    {
        [JsonProperty("urlPattern")]
        public string UrlPattern { get; private set; }

        [JsonProperty("delay")]
        public int Delay { get; private set; }
    }
}
