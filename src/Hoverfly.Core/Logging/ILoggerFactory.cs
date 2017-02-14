namespace Hoverfly.Core.Logging
{
    public interface ILoggerFactory
    {
        ILog Create(string name);
    }
}