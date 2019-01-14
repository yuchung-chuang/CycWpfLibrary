using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using CycWpfLibrary.Media;
using CycWpfLibrary;

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
      Assert.AreEqual(PB2, editor.Object);
      editor.Undo();
      Assert.AreEqual(PB1, editor.Object);
      editor.Redo();
      Assert.AreEqual(PB2, editor.Object);
    }
  }
}