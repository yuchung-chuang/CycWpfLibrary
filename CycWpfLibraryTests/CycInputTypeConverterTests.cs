using Microsoft.VisualStudio.TestTools.UnitTesting;
using CycWpfLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CycWpfLibrary.Tests
{
  [TestClass()]
  public class CycInputTypeConverterTests
  {
    [TestMethod()]
    public void ConvertFromTest()
    {
      var converter = new CycInputTypeConverter();
      var str = "mouse ,,left, right,2,3,; key ,  a, bb, ctrlc, leftctrl";
      var input = converter.ConvertFrom(str) as CycInput;
      Assert.IsTrue(input.MouseButton == MouseButton.Right);
      Assert.IsTrue(input.InputKeys[0].Value == Key.A);
      Assert.IsTrue(input.InputKeys[1].Value == Key.LeftCtrl);
      Assert.IsTrue(input.ClickCount == 3);

      str = "mouse:left";
      input = converter.ConvertFrom(str) as CycInput;
      Assert.IsTrue(input.MouseButton == MouseButton.Left);
      Assert.IsTrue(input.InputKeys == null);
    }
  }
}