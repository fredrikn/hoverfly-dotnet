namespace Hoverfly.Core.Resources
{
    using System;

    /// <summary>
    /// Throws when exporting simulation data fails.
    /// </summary>
    [Serializable]
    public class SimulationExportException : Exception
    {
        public SimulationExportException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}
