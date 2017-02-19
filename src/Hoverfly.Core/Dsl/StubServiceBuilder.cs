namespace Hoverfly.Core.Dsl
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    using Model;

    public class StubServiceBuilder
    {
        private const string PATCH = "PATCH";

        private readonly string _baseUrl;
        private readonly string _scheme;

        /// <summary>
        /// Instantiates builder for a given base URL 
        /// </summary>
        /// <param name="baseUrl">The base URL of the service you are going to simulate.</param>
        public StubServiceBuilder(string baseUrl) : this(new Uri(baseUrl))
        {
        }

        /// <summary>
        /// Instantiates builder for a given base URL 
        /// </summary>
        /// <param name="baseUrl">The base URL of the service you are going to simulate.</param>
        public StubServiceBuilder(Uri baseUrl)
        {
            if (baseUrl == null)
                throw new ArgumentNullException(nameof(baseUrl));

            _scheme = baseUrl.Scheme;
            _baseUrl = baseUrl.Host;
        }

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
            return RequestMatcherBuilder.CreateRequestMatcherBuilder(this, HttpMethod.Get, _scheme, _baseUrl, path);
        }

        /// <summary>
        /// Creating a DELETE request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Delete(string path)
        {
            return RequestMatcherBuilder.CreateRequestMatcherBuilder(this, HttpMethod.Delete, _scheme, _baseUrl, path);
        }

        /// <summary>
        /// Creating a PUT request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>

        public RequestMatcherBuilder Put(string path)
        {
            return RequestMatcherBuilder.CreateRequestMatcherBuilder(this, HttpMethod.Put, _scheme, _baseUrl, path);
        }

        /// <summary>
        /// Creating a POST request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>

        public RequestMatcherBuilder Post(string path)
        {
            return RequestMatcherBuilder.CreateRequestMatcherBuilder(this, HttpMethod.Post, _scheme, _baseUrl, path);
        }

        /// <summary>
        /// Creating a PATCH request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>

        public RequestMatcherBuilder Patch(string path)
        {
            return RequestMatcherBuilder.CreateRequestMatcherBuilder(this, new HttpMethod(PATCH), _scheme, _baseUrl, path);
        }

        /// <summary>
        /// Adds a pair to this builder.
        /// </summary>
        /// <param name="requestResponsePair">The <see cref="RequestResponsePairs"/> to add.</param>
        /// <returns>Return this instance of the <see cref="StubServiceBuilder"/> for further customizations.</returns>
        public StubServiceBuilder AddRequestResponsePair(RequestResponsePair requestResponsePair)
        {
            RequestResponsePairs.Add(requestResponsePair);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="DelaySettings"/> to this builder.
        /// </summary>
        /// <param name="urlPattern">The urlPattern to set a delay to.</param>
        /// <param name="delay">The delay in milliseconds.</param>
        /// <param name="httpMethod">The http method for the delay.</param>
        /// <returns>Return this instance of the <see cref="StubServiceBuilder"/> for further customizations.</returns>
        public StubServiceBuilder AddDelay(string urlPattern, int delay, HttpMethod httpMethod)
        {
            Delays.Add(new DelaySettings(urlPattern, delay, httpMethod.ToString()));
            return this;
        }
    }
}