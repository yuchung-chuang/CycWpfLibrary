using CycWpfLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycWpfLibrary.Tests
{
  public enum MyEnum
  {
    a = 1,
    b = 2,
    c = 4,
  }
  [TestClass()]
  public class EnumExtensionTests
  {
    [TestMethod()]
    public void EnumTest()
    {
      MyEnum A = (MyEnum)MyEnum.a.Add(MyEnum.b);
      var B = MyEnum.b;
      Assert.IsTrue(A.Contain(B));
      A = (MyEnum)A.Remove(MyEnum.a);
      Assert.AreEqual(A, B);
    }

    [TestMethod()]
    public void GetAllTest()
    {
      var array = EnumHelpers.GetAll<MyEnum>();
      Assert.AreEqual(array[0], MyEnum.a);
      Assert.AreEqual(array[1], MyEnum.b);
      Assert.AreEqual(array[2], MyEnum.c);
    }

    [TestMethod()]
    public void CountTest()
    {
      var count = EnumHelpers.Count<MyEnum>();
      Assert.AreEqual(count, 3);
    }
  }


}