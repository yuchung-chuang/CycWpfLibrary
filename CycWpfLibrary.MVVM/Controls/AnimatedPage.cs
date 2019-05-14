using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;

namespace CycWpfLibrary.MVVM
{
  public enum PageSlideType
  {
    Left,
    Right,
    Top,
    Bottom,
  }
  public enum PageTransitionType
  {
    In,
    Out,
  }
  public enum PageAnimationType
  {
    None = 0,
    Fade = 1,
    Slide = 2,
    FadeSlide = Fade | Slide,
  }

  public class AnimatedPage : ObservableUserControl
  {
    public AnimatedPage()
    {
      Loaded += AnimatedPage_Loaded;
    }

    public double AnimationSeconds { get; set; } = 1;

    public PageAnimationType AnimationType { get; set; } = PageAnimationType.FadeSlide;
    public PageTransitionType TransitionType { get; set; }
    public PageSlideType SlideType { get; set; }

    public Dictionary<(PageSlideType, PageTransitionType), (Thickness From, Thickness To)> FromToPairs { get; private set; } = new Dictionary<(PageSlideType, PageTransitionType), (Thickness From, Thickness To)>();

    public void PageAnimation()
    {
      var width = ActualWidth;
      var height = ActualHeight;
      if (FromToPairs.Count == 0)
        if (width != 0 && height != 0)
          SetFromToPairs();
        else
          return;
      var storyboard = new Storyboard();
      storyboard.AddPageAnimation(this);
      storyboard.Completed += (s,e) => AnimationCompleted?.Invoke(this, e);
      storyboard.Begin(this);

      void SetFromToPairs()
      {
        FromToPairs[(PageSlideType.Left, PageTransitionType.In)] =
          (new Thickness(-width, 0, width, 0), new Thickness(0));
        FromToPairs[(PageSlideType.Right, PageTransitionType.In)] =
          (new Thickness(width, 0, -width, 0), new Thickness(0));
        FromToPairs[(PageSlideType.Top, PageTransitionType.In)] =
          (new Thickness(0, -height, 0, height), new Thickness(0));
        FromToPairs[(PageSlideType.Bottom, PageTransitionType.In)] =
          (new Thickness(0, height, 0, -height), new Thickness(0));
        FromToPairs[(PageSlideType.Left, PageTransitionType.Out)] =
          (new Thickness(0), new Thickness(-width, 0, width, 0));
        FromToPairs[(PageSlideType.Right, PageTransitionType.Out)] =
          (new Thickness(0), new Thickness(width, 0, -width, 0));
        FromToPairs[(PageSlideType.Top, PageTransitionType.Out)] =
          (new Thickness(0), new Thickness(0, -height, 0, height));
        FromToPairs[(PageSlideType.Bottom, PageTransitionType.Out)] =
          (new Thickness(0), new Thickness(0, height, 0, -height));
      }
    }

    public event EventHandler AnimationCompleted;

    private void AnimatedPage_Loaded(object sender, RoutedEventArgs e)
    {
      PageAnimation();
    }
  }
}
