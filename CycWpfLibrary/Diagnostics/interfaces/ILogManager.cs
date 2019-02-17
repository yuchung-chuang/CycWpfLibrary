using System;
using System.Runtime.CompilerServices;

namespace CycWpfLibrary
{
  /// <summary>
  /// Holds a bunch of loggers to log messages for the user
  /// </summary>
  public interface ILogManager
  {
    LogOutputLevel LogOutputLevel { get; set; }
    bool IsLogPosition { get; set; }

    event Action<(string Message, LogLevel Level)> NewLog;
    void AddLogger(ILogger logger);
    void RemoveLogger(ILogger logger);
    void Log(string message, LogLevel level = LogLevel.Informative, [CallerMemberName]string origin = "", [CallerFilePath]string filePath = "", [CallerLineNumber]int lineNumber = 0);
  }
}
