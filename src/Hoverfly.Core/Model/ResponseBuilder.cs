//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;



///// <summary>
///// Adds a dealy to the Request.
///// </summary>
///// <param name="milleseconds">The delay of the request in milliseconds.</param>
///// <returns>Returns this <see cref="RequestMatcherBuilder"/> for further customizations.</returns>
///// <remarks>NOTE: Only the Host, Http method and path will be added to the delay settings. Not the header or the body etc.</remarks>
//public RequestMatcherBuilder WithDelay(int milleseconds)
//{
//    _dealy = milleseconds;
//    var tmpPath = _path.Value;
//    var tmpBaseUrl = _baseUrl.Value;

//    if (tmpPath.StartsWith("/")) tmpPath = tmpPath.Remove(0, 1);
//    if (tmpBaseUrl.EndsWith("/")) tmpBaseUrl = tmpBaseUrl.Remove(tmpBaseUrl.Length - 1, 1);

//    var urlPattern = $"{tmpBaseUrl}/{tmpPath}";

//    _invoker.AddDelay(urlPattern, _dealy.Value, _httpMethod);
//    return this;
//}

//namespace Hoverfly.Core.Model
//{
//    class ResponseBuilder
//    {
//        /**
// * A builder for building {@link Response}
// *
// * @see ResponseCreators
// */
//        public class ResponseBuilder
//        {

//         private final Map<String, List<String>> headers = new HashMap<>();
//         private String body = "";
//         private int status = 200;
//         private boolean templated = true;
//         private final Map<String, String> transitionsState = new HashMap<>();
//         private final List<String> removesState = new ArrayList<>();

//         private int delay;
//         private TimeUnit delayTimeUnit;

//         ResponseBuilder()
//         {
//         }

//         /**
//          * Instantiates a new instance
//          * @return the builder
//          */
//         @Deprecated
//         public static ResponseBuilder response()
//         {
//             return new ResponseBuilder();
//         }

//         /**
//          * Sets the body
//          * @param body body of the response
//          * @return the {@link ResponseBuilder for further customizations}
//          */
//         public ResponseBuilder body(final String body)
//         {
//             this.body = body;
//             return this;
//         }

//         /**
//          * Sets the status
//          * @param status status of the response
//          * @return the {@link ResponseBuilder for further customizations}
//          */
//         public ResponseBuilder status(final int status)
//         {
//             this.status = status;
//             return this;
//         }

//         /**
//          * Sets a header
//          * @param key header name
//          * @param value header value
//          * @return the {@link ResponseBuilder for further customizations}
//          */
//         public ResponseBuilder header(final String key, final String value)
//         {
//             this.headers.put(key, singletonList(value));
//             return this;
//         }

//         /**
//          * Sets a transition state
//          * @param key state key
//          * @param value state value
//          * @return the {@link ResponseBuilder for further customizations}
//          */
//         public ResponseBuilder andSetState(final String key, final String value)
//         {
//             this.transitionsState.put(key, value);
//             return this;
//         }

//         /**
//          * Sets state to be removed
//          * @param stateToRemove a state to be removed
//          * @return the {@link ResponseBuilder for further customizations}
//          */
//         public ResponseBuilder andRemoveState(final String stateToRemove)
//         {
//             this.removesState.add(stateToRemove);
//             return this;
//         }

//         /**
//          * Builds a {@link Response}
//          * @return the response
//          */
//         Response build()
//         {
//             return new Response(status, body, false, templated, headers, transitionsState, removesState);
//         }

//         public ResponseBuilder body(final HttpBodyConverter httpBodyConverter)
//         {
//             this.body = httpBodyConverter.body();
//             this.header("Content-Type", httpBodyConverter.contentType());
//             return this;
//         }


//         public ResponseBuilder disableTemplating()
//         {
//             this.templated = false;
//             return this;
//         }

//         /**
//          * Sets delay parameters.
//          * @param delay amount of delay
//          * @param delayTimeUnit time unit of delay (e.g. SECONDS)
//          * @return the {@link ResponseBuilder for further customizations}
//          */
//         public ResponseBuilder withDelay(int delay, TimeUnit delayTimeUnit)
//         {
//             this.delay = delay;
//             this.delayTimeUnit = delayTimeUnit;
//             return this;
//         }

//         ResponseDelaySettingsBuilder addDelay()
//         {
//             return new ResponseDelaySettingsBuilder(delay, delayTimeUnit);
//         }
//    }
//}
