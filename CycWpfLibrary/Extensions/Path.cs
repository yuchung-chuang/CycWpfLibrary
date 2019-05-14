using System;
using System.Runtime.InteropServices;
using System.IO;

namespace CycWpfLibrary
{
  public static class PathExtensions
  {
    public static string NormalizePath(this string path)
    {
      // If on Windows...
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        return path?.Replace('/', '\\').Trim();
      // If on Linux/Mac
      else
        return path?.Replace('\\', '/').Trim();
    }

    public static string AbsolutePath(this string path) => Path.GetFullPath(path);

    /// <summary>
    /// 取得<paramref name="path"/>之<see cref="Uri"/>物件
    /// </summary>
    /// <param name="path">應用程式資料夾內之路徑</param>
    public static Uri PackUri(this string path)
    {
      return new Uri($"pack://application:,,,/" + path);
    }

  }
}
