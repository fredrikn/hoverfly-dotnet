namespace Hoverfly.Dsl
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    using Core.Model;

    using static RequestMatcherBuilder;

    public class StubServiceBuilder
    {
        private const string PATCH = "PATCH";

        private readonly string _destination;
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
            _destination = baseUrl.Host;
        }

        /// <summary>
        /// Gets all the requestResponsePairs that the builder contains.
        /// </summary>
        public IList<RequestResponsePair> RequestResponsePairs { get; } = new List<RequestResponsePair>();

        /// <summary>
        /// Creating a GET request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Get(string path)
        {
            return CreateRequestMatcherBuilder(this, HttpMethod.Get, _scheme, _destination, path);
        }

        /// <summary>
        /// Creating a DELETE request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
        public RequestMatcherBuilder Delete(string path)
        {
            return CreateRequestMatcherBuilder(this, HttpMethod.Delete, _scheme, _destination, path);
        }

        /// <summary>
        /// Creating a PUT request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>

        public RequestMatcherBuilder Put(string path)
        {
            return CreateRequestMatcherBuilder(this, HttpMethod.Put, _scheme, _destination, path);
        }

        /// <summary>
        /// Creating a POST request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>

        public RequestMatcherBuilder Post(string path)
        {
            return CreateRequestMatcherBuilder(this, HttpMethod.Post, _scheme, _destination, path);
        }

        /// <summary>
        /// Creating a PATCH request matcher
        /// </summary>
        /// <param name="path">The path you want the matcher to have.</param>
        /// <returns>Returns the <see cref="RequestMatcherBuilder"/> for further customizations.</returns>

        public RequestMatcherBuilder Patch(string path)
        {
            return CreateRequestMatcherBuilder(this, new HttpMethod(PATCH), _scheme, _destination, path);
        }

        /// <summary>
        /// Adds a pair to this builder.
        /// </summary>
        /// <param name="requestResponsePair">The <see cref="RequestResponsePairs"/> to add.</param>
        /// <returns>Return this instance of the <see cref="StubServiceBuilder"/>.</returns>
        public StubServiceBuilder AddRequestResponsePair(RequestResponsePair requestResponsePair)
        {
            RequestResponsePairs.Add(requestResponsePair);
            return this;
        }
    }
}