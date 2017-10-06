using Newtonsoft.Json;

namespace Hoverfly.Core.Resources
{
    /// <summary>
    /// Hoverfly mode arguments
    /// </summary>
    public class ModeArguments
    {
        public ModeArguments()
        {
        }

        public ModeArguments(params string[] headersWhitelist)
        {
            HeadersWhitelist = headersWhitelist;
        }

        [JsonProperty("headersWhitelist", NullValueHandling = NullValueHandling.Ignore)]
        public string[] HeadersWhitelist { get; set; }

        [JsonProperty("matchingStrategy", NullValueHandling = NullValueHandling.Ignore)]
        public string MatchingStrategy { get; set; }
    }
}