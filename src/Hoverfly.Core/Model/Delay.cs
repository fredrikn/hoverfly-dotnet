namespace Hoverfly.Core.Model
{
    using System;

    using Newtonsoft.Json;

    public class DelaySettings
    {
        public DelaySettings(string urlPattern, int delay)
        {
            if (string.IsNullOrWhiteSpace(urlPattern))
                throw new ArgumentNullException(nameof(urlPattern));

            UrlPattern = urlPattern;
            Delay = delay;
        }

        [JsonProperty("urlPattern")]
        public string UrlPattern { get; set; }

        [JsonProperty("delay")]
        public int Delay { get; set; }
    }
}
