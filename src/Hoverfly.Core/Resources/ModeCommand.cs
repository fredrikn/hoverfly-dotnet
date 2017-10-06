using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hoverfly.Core.Resources
{
    /// <summary>
    /// Hoverfly mode.
    /// </summary>
    public class ModeCommand
    {
        [JsonProperty("arguments", NullValueHandling = NullValueHandling.Ignore)]
        public ModeArguments Arguments { get; internal set; }

        [JsonProperty("mode", NullValueHandling = NullValueHandling.Ignore)]
        public HoverflyMode Mode { get; internal set; }

        public ModeCommand()
        {
        }

        public ModeCommand(HoverflyMode mode)
        {
            Mode = mode;
        }

        public ModeCommand(HoverflyMode mode, ModeArguments arguments)
        {
            Mode = mode;
            Arguments = arguments;
        }
    }
}