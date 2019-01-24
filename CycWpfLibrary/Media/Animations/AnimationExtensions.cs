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
    /// <param name="sec">動畫的時長。</param>
    public static void AnimateTo<PropertyType>(this IAnimatable animatable, DependencyProperty dp, PropertyType to, double sec)
    {
      DependencyObject animation;
      var duration = TimeSpan.FromSeconds(sec);
      switch (to)
      {
        case double d:
          animation = new DoubleAnimation(d, duration);
          break;
        case Color color:
          animation = new ColorAnimation(color, duration);
          break;
        case Thickness thickness:
          animation = new ThicknessAnimation(thickness, duration);
          break;
        default:
          throw new NotSupportedException();
      }
      animatable.BeginAnimation(dp, animation as AnimationTimeline);
    }

    private static readonly double decelerationRatio = 0.9;
    public static void AddPageSlide(this Storyboard storyboard, AnimatablePage page)
    {
      var (fromThickness, toThickness) = page.FromToPairs[(page.SlideType, page.TransitionType)];

      var animation = new ThicknessAnimation
      {
        From = fromThickness,
        To = toThickness,
        Duration = TimeSpan.FromSeconds(page.AnimationTime),
        DecelerationRatio = decelerationRatio,
      };

      Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));
      storyboard.Children.Add(animation);
    }

    public static void AddPageFade(this Storyboard storyboard, AnimatablePage page)
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
        Duration = TimeSpan.FromSeconds(page.AnimationTime),
        DecelerationRatio = decelerationRatio,
      };

      Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
      storyboard.Children.Add(animation);
    }

    public static void AddPageAnimation(this Storyboard storyboard, AnimatablePage page)
    {
      if (page.AnimationType.Contain(PageAnimationType.Fade))
        AddPageFade(storyboard, page);
      if (page.AnimationType.Contain(PageAnimationType.Slide))
        AddPageSlide(storyboard, page);
    }
  }
}
