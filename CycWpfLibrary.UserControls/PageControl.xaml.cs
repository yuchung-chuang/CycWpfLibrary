using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

      TurnBackButton = BackButton;
      TurnNextButton = NextButton;
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

    public Button TurnNextButton { get; private set; }
    public Button TurnBackButton { get; private set; }

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
      if (oldPage is AnimatedPage animatedOldPage)
        animatedOldPage.PageAnimation(); //UnLoad animation
      if (newPage is AnimatedPage animatedNewPage)
      {
        animatedNewPage.AnimationCompleted += (obj, arg) => pageControl.PageAnimated?.Invoke(pageControl, arg);
        newPageFrame.Content = animatedNewPage; //invoke Load animation
      }
    }

    public event EventHandler PageAnimated;
  }
}
