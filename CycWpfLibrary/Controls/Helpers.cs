using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycWpfLibrary.Controls
{
  public static class Helpers
  {
    public static string ImageExtensionsString => string.Join(";", ImageExtensionsList);

    public static List<string> ImageExtensionsList => ImageExtensionsLowerCase.Concat(ImageExtensionsUpperCase).ToList();

    private static List<string> ImageExtensionsUpperCase => ImageExtensionsLowerCase.Select(s => s.ToUpper()).ToList();

    private static readonly List<string> ImageExtensionsLowerCase = new List<string>
    {
      ".jpg",
      ".bmp",
      ".png",
    };
  }
}
