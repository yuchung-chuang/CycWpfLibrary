using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace CycWpfLibrary
{
  /// <summary>
  /// The standard log factory for Fasetto Word
  /// Logs details to the Debug by default
  /// </summary>
  public class LogManager : ILogManager
  {
    protected List<ILogger> mLoggers = new List<ILogger>();
    protected object mLoggersLock = new object();

    public LogOutputLevel LogOutputLevel { get; set; }
    public bool IsLogPosition { get; set; }
    public event Action<(string Message, LogLevel Level)> NewLog = (details) => { };
    public LogManager(ILogger[] loggers = null, LogOutputLevel logOutputLevel = 0, bool isLogPosition = true)
    {
      LogOutputLevel = logOutputLevel;
      IsLogPosition = isLogPosition;
      // Add any others passed in
      if (loggers != null)
        foreach (var logger in loggers)
          AddLogger(logger);
    }
    public void AddLogger(ILogger logger)
    {
      // Log the list so it is thread-safe
      lock (mLoggersLock)
      {
        // If the logger is not already in the list...
        if (!mLoggers.Contains(logger))
          // Add the logger to the list
          mLoggers.Add(logger);
      }
    }
    public void RemoveLogger(ILogger logger)
    {
      // Log the list so it is thread-safe
      lock (mLoggersLock)
      {
        // If the logger is in the list...
        if (mLoggers.Contains(logger))
          // Remove the logger from the list
          mLoggers.Remove(logger);
      }
    }
    public void Log(
        string message,
        LogLevel level = LogLevel.Informative,
        [CallerMemberName] string origin = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
      // If we should not log the message as the level is too low...
      if (level.LessThan(LogOutputLevel))
        return;

      // If the user wants to know where the log originated from...
      if (IsLogPosition)
        message = $"{message} [{Path.GetFileName(filePath)} > {origin}() > Line {lineNumber}]";

      // Log to all loggers
      mLoggers.ForEach(logger => logger.Log(message, level));

      // Inform listeners
      NewLog.Invoke((message, level));
    }
  }
}
