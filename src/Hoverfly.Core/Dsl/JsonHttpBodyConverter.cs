namespace Hoverfly.Core.Dsl
{
    using Newtonsoft.Json;

    /// <summary>
    /// Converts a object to a Json body with the content type application/json.
    /// </summary>
    public class JsonHttpBodyConverter : IHttpBodyConverter
    {
        public string Body { get; protected set; }

        public string ContentType { get; protected set; }

        public static IHttpBodyConverter Json(object value)
        {
            return new JsonHttpBodyConverter
                                {
                                    Body = JsonConvert.SerializeObject(value),
                                    ContentType = "application/json"
                                };
        }
    }
}
