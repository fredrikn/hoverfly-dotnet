using System;
using System.Collections.Generic;
using System.Linq;

using Hoverfly.Core.Model;
using static Hoverfly.Core.Dsl.HoverflyMatchers;

namespace Hoverfly.Core.Dsl
{
    public class RequestMatcherBuilder
    {
        private const string CONTENT_TYPE = "Content-Type";

        private readonly StubServiceBuilder _invoker;

        private readonly IList<RequestFieldMatcher> _httpMethod;
        private readonly IList<RequestFieldMatcher> _path;
        private readonly IList<RequestFieldMatcher> _baseUrl;
        private readonly IList<RequestFieldMatcher> _scheme;
        private IList<RequestFieldMatcher> _body = new List<RequestFieldMatcher>() { RequestFieldMatcher.NewExactMatcher("") };
        private readonly Dictionary<string, IList<RequestFieldMatcher>> _headers = new Dictionary<string, IList<RequestFieldMatcher>>();
        private Dictionary<string, IList<RequestFieldMatcher>> _queryParams = new Dictionary<string, IList<RequestFieldMatcher>>();

        private Dictionary<string, string> _requiresState = new Dictionary<string, string>();

        internal RequestMatcherBuilder(
            StubServiceBuilder invoker,
            IList<RequestFieldMatcher> httpMethod,
            IList<RequestFieldMatcher> scheme,
            IList<RequestFieldMatcher> baseUrl,
            IList<RequestFieldMatcher> path)
        {
            _invoker = invoker;
            _httpMethod = httpMethod;
            _scheme = scheme;
            _baseUrl = baseUrl;
            _path = path;
        }

        internal static RequestMatcherBuilder CreateRequestMatcherBuilder(
            StubServiceBuilder invoker,
            List<RequestFieldMatcher> method,
            List<RequestFieldMatcher> scheme,
            List<RequestFieldMatcher> destination,
            List<RequestFieldMatcher> path)
        {
            if (invoker == null)
                throw new ArgumentNullException(nameof(invoker));

            if (method == null)
                throw new ArgumentNullException(nameof(method));

            if (scheme == null)
                throw new ArgumentNullException(nameof(scheme));

            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            return new RequestMatcherBuilder(invoker, method, scheme, destination, path);
        }


        /// <summary>
        /// Matches any body.
        /// </summary>
        /// <returns>Returns this <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder AnyBody()
        {
            _body = null;
            return this;
        }

        /// <summary>
        /// Sets the request body
        /// </summary>
        /// <param name="body">The request body to match on.</param>
        /// <returns>Returns this <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Body(string body)
        {
            return Body(EqualsTo(body));
        }

        /// <summary>
        /// Sets the request body using <see cref="IHttpBodyConverter"/>.
        /// </summary>
        /// <param name="httpBodyConverter">Custom http body converter.</param>
        /// <returns>Returns this <see cref="RequestMatcherBuilder"/> for futher customizations.</returns>
        public RequestMatcherBuilder Body(IHttpBodyConverter httpBodyConverter)
        {
            Header(CONTENT_TYPE, httpBodyConverter.ContentType);
            return Body(EqualsTo(httpBodyConverter.Body));
        }

        /// <summary>
        /// Sets the request body
        /// </summary>
        /// <param name="body">The request body to match on.</param>
        /// <returns>Returns this <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Body(RequestFieldMatcher body)
        {
            _body.Add(body);
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
                    _headers[key].Add(EqualsTo(value));
                else
                    _headers[key] = new List<RequestFieldMatcher> { EqualsTo(value) };
            }
            return this;
        }

        /// <summary>
        /// Sets one request header.
        /// </summary>
        /// <param name="key">The header key to match on.</param>
        /// <param name="values">The header values to match on.</param>
        /// <returns>Returns this <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Header(string key, params RequestFieldMatcher[] values)
        {
            foreach (var value in values)
            {
                if (_headers.ContainsKey(key))
                    _headers[key].Add(value);
                else
                    _headers[key] = new List<RequestFieldMatcher> { value };
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
            // TODO Until Hoverfly doesn't has an array matcher we need to do this, hoverfly currently match on array values that are joined by semicolon

            if (values == null || !values.Any())
                _queryParams.Add(key, new List<RequestFieldMatcher> { Any() });
            else
                _queryParams[key] = new List<RequestFieldMatcher> {
                    EqualsTo(string.Join(";", values.Select( v => v.ToString())))
                };

            return this;
        }

        /// <summary>
        /// Sets the request query.
        /// </summary>
        /// <param name="key">The query param key to match on.</param>
        /// <param name="value">The query params values to match on.</param>
        /// <returns>Returns this <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder QueryParam(string key, RequestFieldMatcher value)
        {
            _queryParams.Add(key, new List<RequestFieldMatcher> { value });
            return this;
        }

        /// <summary>
        /// Add a matcher that matches any query parameters
        /// </summary>
        /// <param name="key">The query params key to match on.</param>
        /// <returns>Returns this <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder AnyQueryParams()
        {
            _queryParams = null;
            return this;
        }

        /// <summary>
        /// Sets a required state
        /// </summary>
        /// <param name="key">State key.</param>
        /// <param name="value">State value</param>
        /// <returns>Returns <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder WithState(string key, string value)
        {
            _requiresState.Add(key, value);
            return this;
        }

        /// <summary>
        /// Sets the expected response.
        /// </summary>
        /// <param name="responseBuilder">The builder for response.</param>
        /// <returns>Returns <see cref="StubServiceBuilder"/> for chaining the next <see cref="RequestMatcherBuilder"/>.</returns>
        public StubServiceBuilder WillReturn(ResponseBuilder responseBuilder)
        {
            var request = Build();

            return _invoker.AddRequestResponsePair(new RequestResponsePair(request, responseBuilder.Build()))
                            .AddDelaySetting(request, responseBuilder);
        }

        private Request Build()
        {
            return new Request(_path, _httpMethod, _baseUrl, _scheme, _body, _headers, _queryParams);
        }
    }
}