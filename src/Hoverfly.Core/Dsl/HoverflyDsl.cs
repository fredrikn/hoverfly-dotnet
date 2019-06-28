namespace Hoverfly.Core.Dsl
{
    using global::Hoverfly.Core.Model;
    using System;

    /// <summary>
    /// Entry point to a DSL which can be used to generate a Hoverfly simulation. 
    /// </summary>
    public static class HoverflyDsl
    {
        /// <summary>
        /// Instantiates a DSL for a given service.
        /// </summary>
        /// <param name="baseUrl">The base URL you want all subsequent requests mappings to contain</param>
        /// <returns>Returns <see cref="StubServiceBuilder}"/></returns>
        public static StubServiceBuilder Service(Uri baseUrl)
        {
            return new StubServiceBuilder(baseUrl);
        }

        /// <summary>
        /// Instantiates a DSL for a given service.
        /// </summary>
        /// <param name="baseUrl">The base URL you want all subsequent requests mappings to contain</param>
        /// <returns>Returns <see cref="StubServiceBuilder}"/></returns>
        public static StubServiceBuilder Service(string baseUrl)
        {
            return new StubServiceBuilder(baseUrl);
        }

        /// <summary>
        /// Instantiates a DSL for a given service.
        /// </summary>
        /// <param name="baseUrl">The base URL you want all subsequent requests mappings to contain by using a <see cref="RequestFieldMatcher"/></param>
        /// <returns>Returns <see cref="StubServiceBuilder}"/></returns>
        public static StubServiceBuilder Service(RequestFieldMatcher matcher)
        {
            return new StubServiceBuilder(matcher);
        }

        ///<summary>Instantiates a new instance of <see cref="ResponseBuilder"/></summary>
        ///<returns>Returns <see cref="ResponseBuilder"/></returns>
        public static ResponseBuilder Response()
        {
            return new ResponseBuilder();
        }
    }
}