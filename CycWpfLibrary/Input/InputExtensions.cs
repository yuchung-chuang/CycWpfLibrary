using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CycWpfLibrary
{
  public static class InputExtensions
  {
/// <summary>
    /// 取得滑鼠相對於<paramref name="element"/>在螢幕上的座標，此結果不會受到<see cref="UIElement.RenderTransform"/>的影響。
    /// </summary>
    public static Point GetAbsolutePosition(this MouseEventArgs e, UIElement element)
    {
      var transformsTemplate = (element.RenderTransform as TransformGroup).Children;
      var transformsIdentity = new TransformCollection();
      // 重設UIElement的transforms
      (element.RenderTransform as TransformGroup).Children = transformsIdentity;
      // 取得座標
      var absolute = e.GetPosition(element);
      // 復原transforms
      (element.RenderTransform as TransformGroup).Children = transformsTemplate;
      return absolute;
    }

    public static bool IsPressed(this MouseButton mouseButton)
    {
      switch (mouseButton)
      {
        case MouseButton.Left:
          return Mouse.LeftButton == MouseButtonState.Pressed;
        case MouseButton.Middle:
          return Mouse.MiddleButton == MouseButtonState.Pressed;
        case MouseButton.Right:
          return Mouse.RightButton == MouseButtonState.Pressed;
        case MouseButton.XButton1:
          return Mouse.XButton1 == MouseButtonState.Pressed;
        case MouseButton.XButton2:
          return Mouse.XButton2 == MouseButtonState.Pressed;
        default:
          return false;
      }
    }

    public static bool IsEmpty(this Key key) => key == Key.None ? true : false;
    public static bool IsPressed(this Key key) => Keyboard.GetKeyStates(key).Contain(KeyStates.Down) ? true : false;
  }
}
