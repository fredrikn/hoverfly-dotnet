namespace Hoverfly.Core.Dsl
{
    using System.Collections.Generic;
    using System.Net;

    using Model;

    public class ResponseBuilder
    {
        private readonly Dictionary<string, IList<string>> _headers = new Dictionary<string, IList<string>>();
        private string _body = "";
        private int _status = 200;
        private bool _templated = true;
        private int _delay;

        private readonly Dictionary<string, string> _transitionsState = new Dictionary<string, string>();
        private readonly List<string> _removesState = new List<string>();

        internal ResponseBuilder()
        {
        }

        /// <summary>
        /// Creates a new <see cref="ResponseBuilder"/>.
        /// </summary>
        /// <returns>Returns a new instance of <see cref="ResponseBuilder"/>.</returns>
        public static ResponseBuilder Response()
        {
            return new ResponseBuilder();
        }

        /// <summary>
        /// Sets the body.
        /// </summary>
        /// <param name="body">The body of the response.</param>
        /// <returns>Returns this <see cref="ResponseBuilder"/> for further customizations.</returns>
        public ResponseBuilder Body(string body)
        {
            _body = body;
            return this;
        }

        /// <summary>
        /// Sets the status of the response.
        /// </summary>
        /// <param name="status">The status of the response.</param>
        /// <returns>Returns this <see cref="ResponseBuilder"/> for further customizations.</returns>
        public ResponseBuilder Status(HttpStatusCode status)
        {
            _status = (int)status;
            return this;
        }

        /// <summary>
        /// Sets the status of the response.
        /// </summary>
        /// <param name="status">The status of the response.</param>
        /// <returns>Returns this <see cref="ResponseBuilder"/> for further customizations.</returns>
        public ResponseBuilder Status(int status)
        {
            _status = status;
            return this;
        }


        /// <summary>
        /// Sets a transition state
        /// </summary>
        /// <param name="key">State key</param>
        /// <param name="value">State value</param>
        /// <returns>Returns this <see cref="ResponseBuilder"/> for further customizations.</returns>
        public ResponseBuilder AndSetState(string key, string value)
        {
            _transitionsState.Add(key, value);
            return this;
        }

        /// <summary>
        /// Sets state to be removed
        /// </summary>
        /// <param name="stateToRemove">A state to be removed</param>
        /// <returns>Returns this <see cref="ResponseBuilder"/> for further customizations.</returns>
        public ResponseBuilder AndRemoveState(string stateToRemove)
        {
            _removesState.Add(stateToRemove);
            return this;
        }

        /// <summary>
        /// Sets a header.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="values"></param>
        /// <returns>Returns this <see cref="ResponseBuilder"/> for further customizations.</returns>
        public ResponseBuilder Header(string key, params string[] values)
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
        /// Sets the response body and the matching Content-Type header using <see cref="IHttpBodyConverter"/>.
        /// </summary>
        /// <param name="httpBodyConverter">Custom http body converter.</param>
        /// <returns>Returns this <see cref="ResponseBuilder"/> for futher customizations.</returns>
        public ResponseBuilder Body(IHttpBodyConverter httpBodyConverter)
        {
            _body = httpBodyConverter.Body;
            Header("Content-Type", httpBodyConverter.ContentType);
            return this;
        }

        /// <summary>
        /// Sets delay parameters
        /// </summary>
        /// <param name="delay">Amount of delay in milliseconds</param>
        /// <returns></returns>
        public ResponseBuilder WithDelay(int delay)
        {
            _delay = delay;
            return this;
        }

        public ResponseBuilder DisableTemplating()
        {
            _templated = false;
            return this;
        }

        /// <summary>
        /// Builds a <see cref="Response"/>.
        /// </summary>
        /// <returns>Returns a <see cref="Response"/>.</returns>
        public Response Build()
        {
            return new Response(_status, _body, false, _templated, _headers, _transitionsState, _removesState);
        }

        internal ResponseDelaySettingsBuilder AddDelay()
        {
            return new ResponseDelaySettingsBuilder(_delay);
        }
    }
}