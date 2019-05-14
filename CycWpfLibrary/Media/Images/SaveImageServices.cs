using System.Drawing;
using System.IO;

namespace CycWpfLibrary
{
  public static class SaveImageServices
  {
    public static void SaveIcon(string bitmapPath)
    {
      var dir = Path.GetDirectoryName(bitmapPath);
      var name = Path.GetFileNameWithoutExtension(bitmapPath);
      var outputPath = dir + @"\" + name + ".ico";
      var bitmap = new Bitmap(bitmapPath);
      var icon = bitmap.ToIcon();
      using (var stream = new FileStream(outputPath, FileMode.OpenOrCreate))
      {
        icon.Save(stream);
      }
    }
  }
}
