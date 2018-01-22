namespace Hoverfly.Core.Logging
{
    using System.Diagnostics;

    /// <summary>
    /// Logger that is using <see cref="System.Diagnostics.Debug"/> to log information.
    /// </summary>
    public class OutputLog : ILog
    {
        public void Info(string message)
        {
            Debug.WriteLine(message);
        }
    }
}