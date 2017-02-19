namespace Hoverfly.Core.Model
{
    using System;

    using Newtonsoft.Json;

    public class DelaySettings
    {
        public DelaySettings(string urlPattern, int delay, string httpMethod = null)
        {
            if (string.IsNullOrWhiteSpace(urlPattern))
                throw new ArgumentNullException(nameof(urlPattern));

            UrlPattern = urlPattern;
            Delay = delay;
            HttpMethod = httpMethod;
        }

        [JsonProperty("urlPattern")]
        public string UrlPattern { get; set; }

        [JsonProperty("delay")]
        public int Delay { get; set; }

        [JsonProperty("httpMethod", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string HttpMethod { get; set; }
    }
}