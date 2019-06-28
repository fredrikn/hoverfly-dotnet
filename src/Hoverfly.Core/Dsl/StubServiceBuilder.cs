namespace Hoverfly.Core.Dsl
{
    using System;
    using System.Collections.Generic;

    using static HoverflyMatchers;

    using Model;
    using System.Collections.ObjectModel;

    public class StubServiceBuilder
    {
        private const string SEPARATOR = "://";
        private readonly IList<RequestFieldMatcher> _scheme;

        /// <summary>
        /// Instantiates builder for a given base URL 
        /// </summary>
        /// <param name="baseUrl">The base URL of the service you are going to simulate.</param>
        public StubServiceBuilder(string baseUrl)
        {
            var elements = baseUrl.Split(new string[] { SEPARATOR }, StringSplitOptions.None);
            if (baseUrl.Contains(SEPARATOR))
            {
                _scheme = new List<RequestFieldMatcher> { RequestFieldMatcher.NewExactMatcher(elements[0]) };
                Destination = new List<RequestFieldMatcher> { RequestFieldMatcher.NewExactMatcher(elements[1]) };
            }
            else
            {
                Destination = new List<RequestFieldMatcher> { RequestFieldMatcher.NewExactMatcher(elements[0]) };
            }
        }

        /// <summary>
        /// Instantiates builder for a given base URL 
        /// </summary>
        /// <param name="baseUrl">The base URL of the service you are going to simulate.</param>
        public StubServiceBuilder(Uri baseUrl)
        {
            if (baseUrl == null)
                throw new ArgumentNullException(nameof(baseUrl));

            _scheme = new List<RequestFieldMatcher> { EqualsTo(baseUrl.Scheme) };
            Destination = new List<RequestFieldMatcher> { EqualsTo(baseUrl.Host) };
        }


        /// <summary>
        /// Instantiates builder for a given base URL 
        /// </summary>
        /// <param name="matcher">The base URL of the service you are going to simulate.</param>
        public StubServiceBuilder(RequestFieldMatcher matcher)
        {
            Destination = new List<RequestFieldMatcher> { matcher };
        }

        internal IList<RequestFieldMatcher> Destination { get; }

        /// <summary>
        /// Gets all the requestResponsePairs that the builder contains.
        /// </summary>
        public IList<RequestResponsePair> RequestResponsePairs { get; } = new List<RequestResponsePair>();

        /// <summary>
        /// Gets all the delay settings that the builder contains.
        /// </summary>
        public IList<DelaySettings> Delays { get; } = new List<DelaySettings>();

        /// <summary>
        /// Creating a GET request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Get(string path)
        {
            return Get(EqualsTo(path));
        }

        /// <summary>
        /// Creating a GET request matcher
        /// </summary>
        /// <param name="path">The path to be used for mathing Get path.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Get(RequestFieldMatcher path)
        {
            return CreateRequestMatcherBuilder(HttpMethod.GET, path);
        }

        /// <summary>
        /// Creating a DELETE request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Delete(string path)
        {
            return Delete(EqualsTo(path));
        }

        /// <summary>
        /// Creating a DELETE request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Delete(RequestFieldMatcher path)
        {
            return CreateRequestMatcherBuilder(HttpMethod.DELETE, path);
        }

        /// <summary>
        /// Creating a PUT request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>

        public RequestMatcherBuilder Put(string path)
        {
            return Put(EqualsTo(path));
        }

        /// <summary>
        /// Creating a PUT request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>

        public RequestMatcherBuilder Put(RequestFieldMatcher path)
        {
            return CreateRequestMatcherBuilder(HttpMethod.PUT, path);
        }

        /// <summary>
        /// Creating a POST request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Post(string path)
        {
            return Post(EqualsTo(path));
        }

        /// <summary>
        /// Creating a POST request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Post(RequestFieldMatcher path)
        {
            return CreateRequestMatcherBuilder(HttpMethod.POST, path);
        }

        /// <summary>
        /// Creating a PATCH request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>

        public RequestMatcherBuilder Patch(string path)
        {
            return Patch(EqualsTo(path));
        }

        /// <summary>
        /// Creating a PATCH request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Patch(RequestFieldMatcher path)
        {
            return CreateRequestMatcherBuilder(HttpMethod.PATCH, path);
        }

        /// <summary>
        /// Creating a OPTIONS request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Options(string path)
        {
            return Options(EqualsTo(path));
        }

        /// <summary>
        /// Creating a OPTIONS request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Options(RequestFieldMatcher path)
        {
            return CreateRequestMatcherBuilder(HttpMethod.OPTIONS, path);
        }

        /// <summary>
        /// Creating a HEAD request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Head(string path)
        {
            return Head(EqualsTo(path));
        }

        /// <summary>
        /// Creating a HEAD request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Head(RequestFieldMatcher path)
        {
            return CreateRequestMatcherBuilder(HttpMethod.HEAD, path);
        }

        /// <summary>
        /// Creating a CONNECT request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Connect(string path)
        {
            return Connect(EqualsTo(path));
        }

        /// <summary>
        /// Creating a CONNECT request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Connect(RequestFieldMatcher path)
        {
            return CreateRequestMatcherBuilder(HttpMethod.CONNECT, path);
        }

        /// <summary>
        /// Creating a TRACE request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Trace(string path)
        {
            return Trace(EqualsTo(path));
        }

        /// <summary>
        /// Creating a TRACE request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Trace(RequestFieldMatcher path)
        {
            return CreateRequestMatcherBuilder(HttpMethod.TRACE, path);
        }

        /// <summary>
        /// Creating a request matcher for any Http methods.
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder AnyMethod(string path)
        {
            return AnyMethod(EqualsTo(path));
        }

        /// <summary>
        /// Creating a request matcher for any Http methods.
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder AnyMethod(RequestFieldMatcher path)
        {
            return CreateRequestMatcherBuilder(HttpMethod.ANY, path);
        }

        ///<summary>Adds service wide delay settings.</summary>
        ///<param name="delay">Amount of delay in milliseconds</param>
        /// <returns>Return <see cref="StubServiceDelaySettingsBuilder"/> for further delay customizations.</returns>
        public StubServiceDelaySettingsBuilder AndDelay(int delay)
        {
            return new StubServiceDelaySettingsBuilder(delay, this);
        }

        ///<summary>Used to initialize <see cref="GlobalActions"/></summary>
        ///<returns>List of <see cref="DelaySettings"/></returns>
        public IEnumerable<DelaySettings> GetDelaySettings()
        {
            return new ReadOnlyCollection<DelaySettings>(Delays);
        }

        /// <summary>
        /// Adds a pair to this builder.
        /// </summary>
        /// <param name="requestResponsePair">The <see cref="RequestResponsePairs"/> to add.</param>
        /// <returns>Return this instance of the <see cref="StubServiceBuilder"/> for further customizations.</returns>
        internal StubServiceBuilder AddRequestResponsePair(RequestResponsePair requestResponsePair)
        {
            RequestResponsePairs.Add(requestResponsePair);
            return this;
        }

        internal void AddDelaySetting(DelaySettings delaySettings)
        {
            if (delaySettings != null)
                Delays.Add(delaySettings);
        }

        internal StubServiceBuilder AddDelaySetting(
            Request request,
            ResponseBuilder responseBuilder)
        {
            responseBuilder.AddDelay().To(this).ForRequest(request);
            return this;
        }

        private RequestMatcherBuilder CreateRequestMatcherBuilder(HttpMethod httpMethod, RequestFieldMatcher path)
        {
            return new RequestMatcherBuilder(
                                            this,
                                            GetRequestFieldMatcher(httpMethod),
                                            _scheme,
                                            Destination,
                                            new List<RequestFieldMatcher> { path });
        }

        private IList<RequestFieldMatcher> GetRequestFieldMatcher(HttpMethod httpMethod)
        {
            List<RequestFieldMatcher> matchers = null;

            if (httpMethod != HttpMethod.ANY)
            {
                matchers = new List<RequestFieldMatcher> { RequestFieldMatcher.NewExactMatcher(httpMethod.ToString()) };
            }

            return matchers;
        }

        public enum HttpMethod
        {
            GET,
            PUT,
            POST,
            DELETE,
            PATCH,
            OPTIONS,
            CONNECT,
            HEAD,
            TRACE,
            ANY
        }
    }
}