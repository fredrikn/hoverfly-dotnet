using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hoverfly.Core.Model
{
    public class RequestFieldMatcher
    {
        public RequestFieldMatcher()
        {
        }

        public RequestFieldMatcher(string exactMatchValue) : this( MatcherType.Exact, exactMatchValue) { }
        
        public RequestFieldMatcher(MatcherType matcher, string value)
        {
            Matcher = matcher;
            Value = value;
        }

        [JsonProperty("matcher")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MatcherType Matcher { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        public static RequestFieldMatcher NewExactMatcher(string value)
        {
            return new RequestFieldMatcher(MatcherType.Exact, value);
        }

        public static RequestFieldMatcher NewGlobMatcher(string value)
        {
            return new RequestFieldMatcher(MatcherType.Glob, value);
        }

        public static RequestFieldMatcher NewRegexMatcher(string value)
        {
            return new RequestFieldMatcher(MatcherType.RegEx, value);
        }

        public static RequestFieldMatcher NewXmlMatcher(string value)
        {
            return new RequestFieldMatcher(MatcherType.Xml, value);
        }

        public static RequestFieldMatcher NewXpathMatcher(string value)
        {
            return new RequestFieldMatcher(MatcherType.XPath, value);
        }

        public static RequestFieldMatcher NewJsonMatcher(string value)
        {
            return new RequestFieldMatcher(MatcherType.JSON, value);
        }

        public static RequestFieldMatcher NewJsonPartialMatcher(string value)
        {
            return new RequestFieldMatcher(MatcherType.JSONPartial, value);
        }

        public static RequestFieldMatcher NewJsonPathMatch(string value)
        {
            return new RequestFieldMatcher(MatcherType.JSONPath, value);
        }
    }
}