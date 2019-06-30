namespace Hoverfly.Core.Logging
{
    public class LoggerFactory : ILoggerFactory
    {
        public ILog Create(string name)
        {
            return new OutputLog();
        }
    }
}
