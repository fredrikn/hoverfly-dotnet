using Hoverfly.Core.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Xml;

namespace Hoverfly.Core.Dsl
{
    public static class HoverflyMatchers
    {
        /// <summary>
        /// Create a matcher that exactly equals to the String value of the given object
        /// </summary>
        /// <param name="value">The value to match on</param>
        /// <returns><see cref="RequestFieldMatcher"/></returns>
        public static RequestFieldMatcher EqualsTo(object value)
        {
            return RequestFieldMatcher.NewExactMatcher(value.ToString());
        }

        /// <summary>
        /// Create a matcher that matches a GLOB pattern.
        /// For example:
        /// <pre>HoverflyMatchers.Matches("api-v*.test-svc.*")</pre>
        /// </summary>
        /// <param name="value">The GLOB pattern, use the wildcard character '*' to match any characters</param>
        /// <returns><see cref="RequestFieldMatcher"/></returns>
        public static RequestFieldMatcher Matches(string value)
        {
            return RequestFieldMatcher.NewGlobMatcher(value);
        }

        /// <summary>
        /// Create a matcher that matches a Golang regex pattern.
        /// As the Hoverfly core project is written in Golang, this method is provided as a temporary solution to use the
        /// regex matcher using native Golang regex patterns.
        /// Although there are some variations from the Java regex, majority of the syntax is similar.
        /// see <a href="https://regex-golang.appspot.com/assets/html/index.html">Golang regex quick reference</a>
        /// </summary>
        /// <param name="regexPattern">The Golang regex pattern</param>
        /// <returns><see cref="RequestFieldMatcher"/></returns>
        public static RequestFieldMatcher MatchesGoRegex(string regexPattern)
        {
            return RequestFieldMatcher.NewRegexMatcher(regexPattern);
        }

        /// <summary>
        /// Create a matcher that matches on a string prefixed with the given value
        /// </summary>
        /// <param name="value">The value to start with</param>
        /// <returns><see cref="RequestFieldMatcher"/></returns>
        public static RequestFieldMatcher StartsWith(string value)
        {
            return RequestFieldMatcher.NewRegexMatcher($"^{value}.*");
        }

        /// <summary>
        /// Create a matcher that matches on a string post-fixed with the given value
        /// </summary>
        /// <param name="value">Tthe value to end with</param>
        /// <returns><see cref="RequestFieldMatcher"/></returns>
        public static RequestFieldMatcher EndsWith(string value)
        {
            return RequestFieldMatcher.NewRegexMatcher($".*{value}$");
        }

        /// <summary>
        /// Create a matcher that matches on a string containing the given value
        /// </summary>
        /// <param name="value">The value to contain</param>
        /// <returns><see cref="RequestFieldMatcher"/></returns>
        public static RequestFieldMatcher Contains(string value)
        {
            return RequestFieldMatcher.NewRegexMatcher($".*{value}.*");
        }

        /// <summary>
        /// Create a matcher that matches on any value
        /// </summary>
        /// <returns><see cref="RequestFieldMatcher"/></returns>
        public static RequestFieldMatcher Any()
        {
            return RequestFieldMatcher.NewRegexMatcher(".*");
        }

        /// <summary>
        /// Create a matcher that matches on the given JSON
        /// </summary>
        /// <param name="value">The JSON string value</param>
        /// <returns><see cref="RequestFieldMatcher"/></returns>
        public static RequestFieldMatcher EqualsToJson(string value)
        {
            ValidateJson(value);
            return RequestFieldMatcher.NewJsonMatcher(value);
        }

        /// <summary>
        /// Create a matcher that matches on JSON serialized from an object by <see cref="IHttpBodyConverter"/>}
        /// </summary>
        /// <param name="converter">Converter the <see cref="IHttpBodyConverter"/> with an object to be serialized to JSON</param>
        /// <returns><see cref="RequestFieldMatcher"/></returns>
        public static RequestFieldMatcher EqualsToJson(IHttpBodyConverter converter)
        {
            return EqualsToJson(converter.Body);
        }

        /// <summary>
        /// Create a matcher that matches on the given partial JSON document
        /// </summary>
        /// <param name="value">The JSON string value</param>
        /// <returns><see cref="RequestFieldMatcher"/></returns>
        public static RequestFieldMatcher MatchesPartialJson(string value)
        {
            ValidateJson(value);
            return RequestFieldMatcher.NewJsonPartialMatcher(value);
        }

        /// <summary>
        /// Create a matcher that matches on a partial JSON document serialized from a object by <see cref="IHttpBodyConverter"/>
        /// </summary>
        /// <param name="converter">The <see cref="IHttpBodyConverter"/> with an object to be serialized to JSON</param>
        /// <returns><see cref="RequestFieldMatcher"/></returns>
        public static RequestFieldMatcher MatchesPartialJson(IHttpBodyConverter converter)
        {
            return MatchesPartialJson(converter.Body);
        }

        /// <summary>
        /// Create a matcher that matches on the given JsonPath expression
        /// </summary>
        /// <param name="expression">Expression the JsonPath expression</param>
        /// <returns><see cref="RequestFieldMatcher"/></returns>
        public static RequestFieldMatcher MatchesJsonPath(string expression)
        {
            return RequestFieldMatcher.NewJsonPathMatch(expression);
        }

        /// <summary>
        /// Create a matcher that matches on the given XML
        /// </summary>
        /// <param name="value">The XML string value</param>
        /// <returns><see cref="RequestFieldMatcher"/></returns>
        public static RequestFieldMatcher EqualsToXml(String value)
        {
            ValidateXml(value);
            return RequestFieldMatcher.NewXmlMatcher(value);
        }

        /// <summary>
        /// Create a matcher that matches on XML serialized from a JAVA object by <see cref="IHttpBodyConverter"/>
        /// </summary>
        /// <param name="converter">The <see cref="IHttpBodyConverter"/> with an object to be serialized to XML</param>
        /// <returns><see cref="RequestFieldMatcher"/></returns>
        public static RequestFieldMatcher EqualsToXml(IHttpBodyConverter converter)
        {
            return EqualsToXml(converter.Body);
        }

        /// <summary>
        /// Create a matcher that matches on the given XPath expression
        /// </summary>
        /// <param name="converter">The XPath expression</param>
        /// <returns><see cref="RequestFieldMatcher"/></returns>
        public static RequestFieldMatcher MatchesXPath(String expression)
        {
            return RequestFieldMatcher.NewXpathMatcher(expression);
        }


        private static void ValidateJson(string value)
        {
            value = value.Trim();

            if ((value.StartsWith("{") && value.EndsWith("}")) ||
                (value.StartsWith("[") && value.EndsWith("]")))
            {
                try
                {
                    JToken.Parse(value);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Fail to create JSON matcher from invalid JSON string: " + value, ex);
                }
            }
            else
            {
                throw new ArgumentException("Fail to create JSON matcher from invalid JSON string: " + value);
            }
        }

        private static void ValidateXml(string value)
        {
            try
            {
                new XmlDocument().Load(value);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Fail to create XML matcher from invalid XML string: " + value, ex);
            }
        }

    }
}
