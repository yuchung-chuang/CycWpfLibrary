using CycWpfLibrary.UserControls;
using CycWpfLibrary.MVVM;
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
using CycWpfLibrary.Media;

namespace CycWpfLibrary.UserControls
{
  /// <summary>
  /// FrameControl.xaml 的互動邏輯
  /// </summary>
  public partial class PageControl : UserControl
  {
    public PageControl()
    {
      InitializeComponent();
      gridMain.DataContext = this;
    }

    #region Dependency Properties
    public FrameworkElement CurrentPage
    {
      get => (FrameworkElement)GetValue(CurrentPageProperty);
      set => SetValue(CurrentPageProperty, value);
    }
    public static readonly DependencyProperty CurrentPageProperty =
        DependencyProperty.Register(
          nameof(CurrentPage),
          typeof(FrameworkElement),
          typeof(PageControl),
          new PropertyMetadata(OnCurrentPageChanged));

    public ICommand TurnBackCommand
    {
      get => (ICommand)GetValue(TurnBackCommandProperty);
      set => SetValue(TurnBackCommandProperty, value);
    }
    public static readonly DependencyProperty TurnBackCommandProperty = DependencyProperty.Register(
        nameof(TurnBackCommand),
        typeof(ICommand),
        typeof(PageControl),
        new PropertyMetadata(default(ICommand)));

    public ICommand TurnNextCommand
    {
      get => (ICommand)GetValue(TurnNextCommandProperty);
      set => SetValue(TurnNextCommandProperty, value);
    }
    public static readonly DependencyProperty TurnNextCommandProperty = DependencyProperty.Register(
        nameof(TurnNextCommand),
        typeof(ICommand),
        typeof(PageControl),
        new PropertyMetadata(default(ICommand)));
    #endregion

    /// <summary>
    /// For animation
    /// </summary>
    private static void OnCurrentPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var pageControl = d as PageControl;
      var oldPageFrame = pageControl.oldPageFrame;
      var newPageFrame = pageControl.newPageFrame;

      var oldPage = e.OldValue;
      var newPage = e.NewValue;
      newPageFrame.Content = null;
      oldPageFrame.Content = oldPage;
      if (oldPage is AnimatedPage animatedPage)
        animatedPage.PageAnimation();
      newPageFrame.Content = newPage;
    }
  }
}
