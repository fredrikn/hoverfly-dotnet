namespace Hoverfly.Core.Dsl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;

    using Model;

    public class RequestMatcherBuilder
    {
        private const string CONTENT_TYPE = "Content-Type";

        private readonly StubServiceBuilder _invoker;

        private readonly HttpMethod _httpMethod;
        private readonly string _path;
        private readonly string _baseUrl;
        private readonly string _scheme;
        private string _body = string.Empty;
        private readonly Dictionary<string, IList<string>> _headers = new Dictionary<string, IList<string>>();
        private readonly Dictionary<string, IList<string>> _queryParams = new Dictionary<string, IList<string>>();

        private int? _dealy = null;

        protected RequestMatcherBuilder(
            StubServiceBuilder invoker,
            HttpMethod httpMethod,
            string scheme,
            string baseUrl,
            string path)
        {
            _invoker = invoker;
            _httpMethod = httpMethod;
            _scheme = scheme;
            _baseUrl = baseUrl;
            _path = path;
        }

        internal static RequestMatcherBuilder CreateRequestMatcherBuilder(
            StubServiceBuilder invoker,
            HttpMethod method,
            string scheme,
            string destination,
            string path)
        {
            if (invoker == null)
                throw new ArgumentNullException(nameof(invoker));

            if (method == null)
                throw new ArgumentNullException(nameof(method));

            if (string.IsNullOrWhiteSpace(scheme))
                throw new ArgumentNullException(nameof(scheme));

            if (string.IsNullOrWhiteSpace(destination))
                throw new ArgumentNullException(nameof(destination));

            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            return new RequestMatcherBuilder(invoker, method, scheme, destination, path);
        }

        /// <summary>
        /// Sets the request body
        /// </summary>
        /// <param name="body">The request body to match on.</param>
        /// <returns>Returns this <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Body(string body)
        {
            _body = body;
            return this;
        }

        /// <summary>
        /// Sets the request body and the matching Content-Type header using <see cref="IHttpBodyConverter"/>.
        /// </summary>
        /// <param name="httpBodyConverter">Custom http body converter.</param>
        /// <returns>Returns this <see cref="RequestMatcherBuilder"/> for futher customizations.</returns>
        public RequestMatcherBuilder Body(IHttpBodyConverter httpBodyConverter)
        {
            _body = httpBodyConverter.Body;
            Header(CONTENT_TYPE, httpBodyConverter.ContentType);
            return this;
        }

        /// <summary>
        /// Sets one request header.
        /// </summary>
        /// <param name="key">The header key to match on.</param>
        /// <param name="values">The header values to match on.</param>
        /// <returns>Returns this <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Header(string key, params string[] values)
        {
            foreach (var value in values)
            {
                if (_headers.ContainsKey(key))
                    _headers[key].Add(value);
                else
                    _headers[key] = new List<string> { value };
            }
            return this;
        }

        /// <summary>
        /// Sets the request query.
        /// </summary>
        /// <param name="key">The query params key to match on.</param>
        /// <param name="values">The query params values to match on.</param>
        /// <returns>Returns this <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder QueryParam(string key, params object[] values)
        {
            foreach (var value in values)
            {
                if (_queryParams.ContainsKey(key))
                    _queryParams[key].Add(value.ToString());
                else
                    _queryParams[key] = new List<string> { value.ToString() };
            }

            return this;
        }


        /// <summary>
        /// Adds a dealy to the Request.
        /// </summary>
        /// <param name="milleseconds">The delay of the request in milliseconds.</param>
        /// <returns>Returns this <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        /// <remarks>NOTE: Only the Host, Http method and path will be added to the delay settings. Not the header or the body etc.</remarks>
        public RequestMatcherBuilder WithDelay(int milleseconds)
        {
            _dealy = milleseconds;
            var tmpPath = _path;
            var tmpBaseUrl = _baseUrl;

            if (tmpPath.StartsWith("/")) tmpPath = tmpPath.Remove(0, 1);
            if (tmpBaseUrl.EndsWith("/")) tmpBaseUrl = tmpBaseUrl.Remove(tmpBaseUrl.Length-1, 1);

            var urlPattern = $"{tmpBaseUrl}/{tmpPath}";

            _invoker.AddDelay(urlPattern, _dealy.Value, _httpMethod);
            return this;
        }
        
        
        /// <summary>
        /// Sets the expected response.
        /// </summary>
        /// <param name="responseBuilder">The builder for response.</param>
        /// <returns>Returns <see cref="StubServiceBuilder"/> for chaining the next <see cref="RequestMatcherBuilder"/>.</returns>
        public StubServiceBuilder WillReturn(ResponseBuilder responseBuilder)
        {
            return _invoker.AddRequestResponsePair(new RequestResponsePair(Build(), responseBuilder.Build()));
        }

        private Request Build()
        {
            var query = CrateQueryParams();
            return new Request(_path, _httpMethod.ToString(), _baseUrl, _scheme, query, _body, _headers);
        }

        private string CrateQueryParams()
        {
            return string.Join("&",
                _queryParams.Select(item => string.Join("&", item.Value.Select(value => CreateKeyValeuParam(item.Key, value)))));
        }

        private static string CreateKeyValeuParam(string key, string value)
        {
            return $"{WebUtility.UrlEncode(key)}={WebUtility.UrlEncode(value)}";
        }
    }
}