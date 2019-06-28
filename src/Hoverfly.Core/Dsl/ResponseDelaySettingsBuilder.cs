using Hoverfly.Core.Model;
using System;
using static Hoverfly.Core.Dsl.StubServiceBuilder;

namespace Hoverfly.Core.Dsl
{
    internal class ResponseDelaySettingsBuilder : AbstractDelaySettingsBuilder
    {
        private StubServiceBuilder _invoker;

        internal ResponseDelaySettingsBuilder(int delay) : base(delay)
        {
        }

        internal ResponseDelaySettingsBuilder To(StubServiceBuilder invoker)
        {
            _invoker = invoker;
            return this;
        }

        internal void ForRequest(Request request)
        {
            if (IsValid())
            {
                HttpMethod httpMethod;
                HttpMethod? requestHttpMethod = null;

                if (Enum.TryParse(ToPattern(request.Method), out httpMethod))
                {
                    requestHttpMethod = httpMethod;
                }

                var urlPattern = ToPattern(request.Destination) + ToPattern(request.Path);
                _invoker.AddDelaySetting(
                                        new DelaySettings(
                                            urlPattern,
                                            Delay,
                                            requestHttpMethod)
                                        );
            }
        }
    }
}
