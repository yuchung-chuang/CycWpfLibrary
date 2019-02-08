using CycWpfLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CycWpfLibrary.Media
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

    private static readonly double decelerationRatio = 0.9;
    public static void AddPageSlide(this Storyboard storyboard, AnimatedPage page)
    {
      var (fromThickness, toThickness) = page.FromToPairs[(page.SlideType, page.TransitionType)];

      var animation = new ThicknessAnimation
      {
        From = fromThickness,
        To = toThickness,
        Duration = TimeSpan.FromSeconds(page.AnimationSeconds),
        DecelerationRatio = decelerationRatio,
      };

      Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));
      storyboard.Children.Add(animation);
    }

    public static void AddPageFade(this Storyboard storyboard, AnimatedPage page)
    {
      double from = 0, to = 0;
      switch (page.TransitionType)
      {
        case PageTransitionType.In:
          from = 0;
          to = 1;
          break;
        case PageTransitionType.Out:
          from = 1;
          to = 0;
          break;
        default:
          break;
      }

      var animation = new DoubleAnimation
      {
        From = from,
        To = to,
        Duration = TimeSpan.FromSeconds(page.AnimationSeconds),
        DecelerationRatio = decelerationRatio,
      };

      Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
      storyboard.Children.Add(animation);
    }

    public static void AddPageAnimation(this Storyboard storyboard, AnimatedPage page)
    {
      if (page.AnimationType.Contain(PageAnimationType.Fade))
        AddPageFade(storyboard, page);
      if (page.AnimationType.Contain(PageAnimationType.Slide))
        AddPageSlide(storyboard, page);
    }
  }
}
