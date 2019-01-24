using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace CycWpfLibrary.Media
{
  public enum PageSlideType
  {
    Left = 1,
    Right = 2,
    Top = 4,
    Bottom = 8,
  }
  public enum PageTransitionType
  {
    In = 1,
    Out = 2,
  }
  public enum PageAnimationType
  {
    None = 0,
    Fade = 1,
    Slide = 2,
    FadeSlide = Fade | Slide,
  }

  public class AnimatablePage : Page
  {
    public AnimatablePage()
    {
      Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() => 
      {
        var width = ActualWidth;
        var height = ActualHeight;
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
      }));
    }

    public double AnimationTime { get; set; } = 1;

    public PageAnimationType AnimationType { get; set; } = PageAnimationType.FadeSlide;
    public PageTransitionType TransitionType { get; set; }
    public PageSlideType SlideType { get; set; }

    public readonly Dictionary<(PageSlideType, PageTransitionType), (Thickness From, Thickness To)> FromToPairs = new Dictionary<(PageSlideType, PageTransitionType), (Thickness From, Thickness To)>();

    public void BeginPageAnimation()
    {
      var storyboard = new Storyboard();
      storyboard.AddPageAnimation(this);
      storyboard.Begin(this);
      Task.Delay(TimeSpan.FromSeconds(AnimationTime));
    }
  }
}
