using System.Runtime.Serialization;

namespace Hoverfly.Core.Model
{
    public enum MatcherType
    {
        [EnumMember(Value = "exact")]
        Exact,
        [EnumMember(Value = "glob")]
        Glob,
        [EnumMember(Value = "regex")]
        RegEx,
        [EnumMember(Value = "xml")]
        Xml,
        [EnumMember(Value = "xpath")]
        XPath,
        [EnumMember(Value = "json")]
        JSON,
        [EnumMember(Value = "jsonpartial")]
        JSONPartial,
        [EnumMember(Value = "jsonpath")]
        JSONPath
    }
}
