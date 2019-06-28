using Hoverfly.Core.Model;
using static Hoverfly.Core.Dsl.StubServiceBuilder;

namespace Hoverfly.Core.Dsl
{
    public class StubServiceDelaySettingsBuilder : AbstractDelaySettingsBuilder
    {
        private StubServiceBuilder _invoker;

        internal StubServiceDelaySettingsBuilder(int delay, StubServiceBuilder invoker) : base(delay)
        {
            _invoker = invoker;
        }

        public StubServiceBuilder ForAll()
        {
            if (IsValid())
                _invoker.AddDelaySetting(new DelaySettings(ToPattern(_invoker.Destination), Delay, null));

            return _invoker;
        }

        public StubServiceBuilder ForMethod(HttpMethod method)
        {
            if (IsValid())
                _invoker.AddDelaySetting(new DelaySettings(ToPattern(_invoker.Destination), Delay, method));

            return _invoker;
        }
    }
}
