using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CycWpfLibrary
{
  public static class AnimationExtensions
  {
    /// <summary>
    /// 針對<paramref name="animatable"/>執行泛型動畫。
    /// </summary>
    /// <typeparam name="PropertyType">執行動畫的屬性型別。</typeparam>
    /// <param name="animatable">要執行動畫的個體。</param>
    /// <param name="dp">要執行動畫的屬性。</param>
    /// <param name="to">屬性改變的目標值。</param>
    /// <param name="durationMs">動畫的時長。</param>
    public static void BeginAnimation<PropertyType>(this IAnimatable animatable, DependencyProperty dp, PropertyType to, double durationMs)
    {
      DependencyObject animation;
      var duration = TimeSpan.FromMilliseconds(durationMs);
      switch (to)
      {
        case int i:
          animation = new Int32Animation(i, duration);
          break;
        case double d:
          animation = new DoubleAnimation(d, duration);
          break;
        case Color color:
          animation = new ColorAnimation(color, duration);
          break;
        case Thickness thickness:
          animation = new ThicknessAnimation(thickness, duration);
          break;
        case Rect rect:
          animation = new RectAnimation(rect, duration);
          break;
        default:
          throw new NotSupportedException();
      }
      animatable.BeginAnimation(dp, animation as AnimationTimeline);
    }

    public static void BeginAnimation<PropertyType>(this IAnimatable animatable, DependencyProperty dp, PropertyType from, PropertyType to, double durationMs) 
    {
      DependencyObject animation;
      var duration = TimeSpan.FromMilliseconds(durationMs);
      switch (to)
      {
        case int toInt:
          var fromInt = (int)Convert.ChangeType(from, typeof(int));
          animation = new Int32Animation(fromInt, toInt, duration);
          break;
        case double toDouble:
          var fromDouble = (double)Convert.ChangeType(from, typeof(double));
          animation = new DoubleAnimation(fromDouble, toDouble, duration);
          break;
        case Color toColor:
          var fromColor = (Color)Convert.ChangeType(from, typeof(Color));
          animation = new ColorAnimation(fromColor, toColor, duration);
          break;
        case Thickness toThickness:
          var fromThickness = (Thickness)Convert.ChangeType(from, typeof(Thickness));
          animation = new ThicknessAnimation(fromThickness, toThickness, duration);
          break;
        case Rect toRect:
          var fromRect = (Rect)Convert.ChangeType(from, typeof(Rect));
          animation = new RectAnimation(fromRect, toRect, duration);
          break;
        default:
          throw new NotSupportedException();
      }
      animatable.BeginAnimation(dp, animation as AnimationTimeline);
    }
  }
}
