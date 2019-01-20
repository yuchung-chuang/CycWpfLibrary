using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CycWpfLibrary.Media
{
  public static class AnimationExtensions
  {
    public static void AnimateTo<TypeOfTo>(this IAnimatable animatable, DependencyProperty dp, TypeOfTo to, double sec)
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
        default:
          throw new NotSupportedException();
      }
      animatable.BeginAnimation(dp, animation as AnimationTimeline);
    }
  }
}
