namespace Hoverfly.Core.Logging
{
    using System.Diagnostics;

    public class OutputLog : ILog
    {
        public void Info(string message)
        {
            Debug.WriteLine(message);
        }
    }
}