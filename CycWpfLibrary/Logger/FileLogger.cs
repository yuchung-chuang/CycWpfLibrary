using System;
using System.IO;

namespace CycWpfLibrary.Logger
{
  /// <summary>
  /// Logs to a specific file
  /// </summary>
  public class FileLogger : ILogger
  {
    public string FilePath { get; set; }

    public bool LogTime { get; set; } = true;

    public FileLogger(string filePath)
    {
      // Set the file path property
      FilePath = filePath;
    }

    public void Log(string message, LogLevel level)
    {
      // Get current time
      var currentTime = DateTimeOffset.Now.ToString("yyyy-MM-dd hh:mm:ss");

      // Prepend the time to the log if desired
      var timeLogString = LogTime ? $"[{ currentTime}] " : "";

      // Write the message
      File.AppendAllText(FilePath, $"{timeLogString}{message}{Environment.NewLine}");
    }

  }
}
