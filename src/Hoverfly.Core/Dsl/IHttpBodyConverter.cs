namespace Hoverfly.Core.Dsl
{
    /// <summary>
    /// Interface for converting a java object into a http request body, and storing the appropriate content type header value.
    /// </summary>
    public interface IHttpBodyConverter
    {
        /// <summary>
        /// Gets the body.
        /// </summary>
        string Body { get; }

        /// <summary>
        /// Gets the content type.
        /// </summary>
        string ContentType { get; }
    }
}