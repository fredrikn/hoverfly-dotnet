using System;

namespace Hoverfly.Core.Configuration
{
    /// <summary>
    /// Thrown when a TCP/IP Port is already in use.
    /// </summary>
    public class PortAlreadyInUseException : Exception
    {
        public PortAlreadyInUseException(string message, Exception inntException = null) : base(message, inntException)
        {
        }
    }
}
