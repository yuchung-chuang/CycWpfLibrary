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


  }
}
