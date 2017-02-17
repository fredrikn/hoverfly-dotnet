namespace Hoverfly.Dsl
{
    using System.Net;
    using static ResponseBuilder;

    /// <summary>
    /// Wrapper around a <see cref="ResponseBuilder"/> for building common types of responses.
    /// </summary>
    public class ResponseCreators
    {
        private ResponseCreators()
        {
        }

        /// <summary>
        /// Builds a 201 response with a given location header value.
        /// </summary>
        /// <param name="locationHeaderValue">the value of the location header.</param>
        /// <returns></returns>
        public static ResponseBuilder Created(string locationHeaderValue)
        {
            return Response()
                    .Status(HttpStatusCode.Created)
                    .Header("Location", locationHeaderValue);
        }

        /// <summary>
        /// Builds a 200 response with the following content
        /// </summary>
        /// <param name="body">The body sent in the response.</param>
        /// <param name="contentType">The content type header value.</param>
        /// <returns>Returns <see cref="ResponseBuilder"/> with the given arguments set.</returns>
        public static ResponseBuilder Success(string body, string contentType)
        {
            return Response()
                    .Status(HttpStatusCode.OK)
                    .Body(body)
                    .Header("Content-Type", contentType);
        }

        /// <summary>
        /// Builds a 200 response.
        /// </summary>
        /// <returns>Returns <see cref="ResponseBuilder"/> with the given arguments set.</returns>
        public static ResponseBuilder Success()
        {
            return Response().Status(HttpStatusCode.OK);
        }

        /// <summary>
        /// Builds a 204 response.
        /// </summary>
        /// <returns>Returns <see cref="ResponseBuilder"/> with the given arguments set.</returns>
        public static ResponseBuilder NoContent()
        {
            return Response().Status(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Builds a 400 response.
        /// </summary>
        /// <returns>Returns <see cref="ResponseBuilder"/> with the given arguments set.</returns>
        public static ResponseBuilder BadRequest()
        {
            return Response().Status(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Builds a 500 response.
        /// </summary>
        /// <returns>Returns <see cref="ResponseBuilder"/> with the given arguments set.</returns>
        public static ResponseBuilder InternalServerError()
        {
            return Response().Status(HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Builds a 403 response.
        /// </summary>
        /// <returns>Returns <see cref="ResponseBuilder"/> with the given arguments set.</returns>
        public static ResponseBuilder Forbidden()
        {
            return Response().Status(HttpStatusCode.Forbidden);
        }

        /// <summary>
        /// Builds a 401 response.
        /// </summary>
        /// <returns>Returns <see cref="ResponseBuilder"/> with the given arguments set.</returns>
        public static ResponseBuilder Unauthorised()
        {
            return Response().Status(HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// Builds a 200 response with a <see cref="IHttpBodyConverter"/>.
        /// </summary>
        /// <param name="httpBodyConverter"></param>
        /// <returns>Returns <see cref="ResponseBuilder"/> with the given arguments set.</returns>
        public static ResponseBuilder Success(IHttpBodyConverter httpBodyConverter)
        {
            return Response()
                .Status(HttpStatusCode.OK )
                .Body(httpBodyConverter);
        }
    }
}