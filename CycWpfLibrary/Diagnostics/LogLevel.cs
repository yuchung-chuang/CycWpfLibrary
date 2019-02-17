using System;

namespace CycWpfLibrary
{
  public enum LogLevel
  {
    Debug = 1,
    Verbose = 2,
    Warning = 3,
    Error = 4,
    Success = 5,
    Informative = 6,
  }

  /// <summary>
  /// the message will be log only if <see cref="LogOutputLevel"/> is greater than <see cref="LogLevel"/>
  /// </summary>
  public enum LogOutputLevel
  {
    Debug = 1,
    Verbose = 2,
    Informative = 3,
    Critical = 4,
    Nothing = 7, 
  }
}
