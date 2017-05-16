using Newtonsoft.Json;

namespace Hoverfly.Core.Model
{
    public class FieldMatcher
    {
        public FieldMatcher()
        {
        }

        public FieldMatcher(string exactMatchValue)
        {
            ExactMatch = exactMatchValue;
        }

        [JsonProperty("exactMatch", NullValueHandling = NullValueHandling.Ignore)]
        public string ExactMatch { get; set; }

        [JsonProperty("globMatch", NullValueHandling = NullValueHandling.Ignore)]
        public string GlobMatch { get; set; }

        [JsonProperty("jsonMatch", NullValueHandling = NullValueHandling.Ignore)]
        public string JsonMatch { get; set; }

        [JsonProperty("jsonPathMatch", NullValueHandling = NullValueHandling.Ignore)]
        public string JsonPathMatch { get; set; }

        [JsonProperty("regexMatch", NullValueHandling = NullValueHandling.Ignore)]
        public string RegexMatch { get; set; }

        [JsonProperty("xpathMatch", NullValueHandling = NullValueHandling.Ignore)]
        public string XpathMatch { get; set; }

        [JsonProperty("xmlMatch", NullValueHandling = NullValueHandling.Ignore)]
        public string XmlMatch { get; set; }
    }
}