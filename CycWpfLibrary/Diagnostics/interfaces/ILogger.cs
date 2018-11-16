namespace CycWpfLibrary.Diagnostics
{
    /// <summary>
    /// A logger that will handle log messages from a <see cref="ILogManager"/>
    /// </summary>
    public interface ILogger
    {
        void Log(string message, LogLevel level);
    }
}
