using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CycWpfLibrary.MVVM
{
  public static class AnimationExtensions
  {
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
