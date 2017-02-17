namespace Hoverfly.Core.Resources
{
    using System;

    /// <summary>
    /// Throws when simulation data is empty.
    /// </summary>
    [Serializable]
    public class SimulationEmptyException : Exception
    {
        public SimulationEmptyException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}
