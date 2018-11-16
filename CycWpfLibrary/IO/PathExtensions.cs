using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace CycWpfLibrary.IO
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
