using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using CycWpfLibrary;
using System.Linq;

namespace CycWpfLibrary.Tests
{
  [TestClass()]
  public class EditorTests
  {
    [TestMethod()]
    public void EditorTest()
    {
      var PB1 = new Bitmap(@"C:\Users\alex\Desktop\WPF\WpfPlotDigitizer\WpfPlotDigitizerTests\data.png").ToPixelBitmap();
      var PB2 = new Bitmap(@"C:\Users\alex\Desktop\WPF\WpfPlotDigitizer\WpfPlotDigitizer\images\ocr.png").ToPixelBitmap();
      var editor = new EditManager();
      editor.Init(PB1);
      editor.Edit(PB2);
      Assert.IsTrue(PB2.Equals(editor.Object as PixelBitmap));
      editor.Undo();
      Assert.IsTrue(PB1.Equals(editor.Object as PixelBitmap));
      editor.Redo();
      Assert.IsTrue(PB2.Equals(editor.Object as PixelBitmap));
    }
  }
}