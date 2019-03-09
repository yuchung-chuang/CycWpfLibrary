using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CycWpfLibrary.FluentDesign
{
  public class CycButton : Button
  {
    static CycButton()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(CycButton), new FrameworkPropertyMetadata(typeof(CycButton)));
    }

    public bool EnableHoverLight
    {
      get => (bool)GetValue(EnableHoverLightProperty);
      set => SetValue(EnableHoverLightProperty, value);
    }
    public static readonly DependencyProperty EnableHoverLightProperty = DependencyProperty.Register(
        nameof(EnableHoverLight),
        typeof(bool),
        typeof(CycButton),
        new PropertyMetadata(default(bool)));

    public bool EnablePressLight
    {
      get => (bool)GetValue(EnablePressLightProperty);
      set => SetValue(EnablePressLightProperty, value);
    }
    public static readonly DependencyProperty EnablePressLightProperty = DependencyProperty.Register(
        nameof(EnablePressLight),
        typeof(bool),
        typeof(CycButton),
        new PropertyMetadata(default(bool)));

    public double HoverLightSize
    {
      get => (double)GetValue(HoverLightSizeProperty);
      set => SetValue(HoverLightSizeProperty, value);
    }
    public static readonly DependencyProperty HoverLightSizeProperty = DependencyProperty.Register(
        nameof(HoverLightSize),
        typeof(double),
        typeof(CycButton),
        new PropertyMetadata(100d));

    public double PressLightSize
    {
      get => (double)GetValue(PressLightSizeProperty);
      set => SetValue(PressLightSizeProperty, value);
    }
    public static readonly DependencyProperty PressLightSizeProperty = DependencyProperty.Register(
        nameof(PressLightSize),
        typeof(double),
        typeof(CycButton),
        new PropertyMetadata(default(double)));

  }
}
