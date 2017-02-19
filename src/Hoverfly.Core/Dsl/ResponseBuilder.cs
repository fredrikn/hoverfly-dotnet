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

        private ResponseBuilder()
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
        /// Builds a <see cref="Response"/>.
        /// </summary>
        /// <returns>Returns a <see cref="Response"/>.</returns>
        public Response Build()
        {
            return new Response(_status, _body, false, _headers);
        }
    }
}