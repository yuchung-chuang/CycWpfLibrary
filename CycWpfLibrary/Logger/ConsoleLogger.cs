using System;

namespace CycWpfLibrary.Logger
{
  public class ConsoleLogger : ILogger
  {
    public void Log(string message, LogLevel level)
    {
      // Save old color
      var consoleOldColor = Console.ForegroundColor;
      // Default log color value 
      var consoleColor = ConsoleColor.White;
      // Color console based on level
      switch (level)
      {
        case LogLevel.Debug:
          consoleColor = ConsoleColor.Blue;
          break;
        case LogLevel.Verbose:
          consoleColor = ConsoleColor.Gray;
          break;
        case LogLevel.Warning:
          consoleColor = ConsoleColor.DarkYellow;
          break;
        case LogLevel.Error:
          consoleColor = ConsoleColor.Red;
          break;
        case LogLevel.Success:
          consoleColor = ConsoleColor.Green;
          break;
      }
      // Set the desired console color
      Console.ForegroundColor = consoleColor;
      // Write message to console
      Console.WriteLine(message);
      // Reset color
      Console.ForegroundColor = consoleOldColor;
    }
  }
}
