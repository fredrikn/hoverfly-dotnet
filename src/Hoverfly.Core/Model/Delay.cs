namespace Hoverfly.Core.Model
{
    using System;

    using Newtonsoft.Json;
    using static global::Hoverfly.Core.Dsl.StubServiceBuilder;

    public class DelaySettings
    {
        public DelaySettings(string urlPattern, int delay, HttpMethod? httpMethod = null)
        {
            if (string.IsNullOrWhiteSpace(urlPattern))
                throw new ArgumentNullException(nameof(urlPattern));

            UrlPattern = urlPattern;
            Delay = delay;

            if (httpMethod.HasValue)
                HttpMethod = httpMethod.ToString();
        }

        [JsonProperty("urlPattern")]
        public string UrlPattern { get; set; }

        [JsonProperty("delay")]
        public int Delay { get; set; }

        [JsonProperty("httpMethod", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string HttpMethod { get; set; }
    }
}