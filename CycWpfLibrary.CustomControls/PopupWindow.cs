using CycWpfLibrary;
using CycWpfLibrary.Media;
using CycWpfLibrary.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CycWpfLibrary.CustomControls
{
  public class PopupWindow : Window
  {
    static PopupWindow()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupWindow), new FrameworkPropertyMetadata(typeof(PopupWindow)));
    }

    public PopupWindow()
    {
      AllowsTransparency = true;
      WindowStyle = WindowStyle.None;
      SizeToContent = SizeToContent.WidthAndHeight;
    }

    #region DPs
    public string Text
    {
      get => (string)GetValue(TextProperty);
      set => SetValue(TextProperty, value);
    }
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(PopupWindow),
        new PropertyMetadata(default(string)));

    public FrameworkElement PlacementTarget
    {
      get => (FrameworkElement)GetValue(PlacementTargetProperty);
      set => SetValue(PlacementTargetProperty, value);
    }
    public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.Register(
        nameof(PlacementTarget),
        typeof(FrameworkElement),
        typeof(PopupWindow),
        new PropertyMetadata(default(FrameworkElement), OnPlacementTargetChanged));

    private static void OnPlacementTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var popup = d as PopupWindow;

      var mainWindow = Application.Current.MainWindow;
      mainWindow.EnsurePanelContent();
      var mainPanel = mainWindow.Content as Panel;
      var target = popup.PlacementTarget;
      popup.windowPos = target.TransformToAncestor(mainPanel).Transform(new Point());
      popup.screenPos = target.PointToScreenDPI(new Point());
      popup.padding = 5d;
      popup.cornerRadius = 10;
      popup.mainPanel = mainPanel;
    }
    #endregion

    private const string PART_TextBlock_Name = nameof(PART_TextBlock);
    private const string PART_CloseButton_Name = nameof(PART_CloseButton);

    private TextBlock PART_TextBlock;
    private Button PART_CloseButton;
    private Border shadowBorder = new Border
    {
      Background = ResourceManager.ShadowBrush,
    };
    private Panel mainPanel;
    private Point windowPos;
    private Point screenPos;
    private double padding;
    private double cornerRadius;
    private RectangleGeometry rectGeo;
    private Rect initialRect;
    private Rect targetRect;
    private static readonly double enterMillisecond = 500;
    private static readonly Duration enterDuration = enterMillisecond.ToDuration();
    private static readonly double leaveMillisecond = enterMillisecond / 2;
    private static readonly Duration leaveDuration = leaveMillisecond.ToDuration();
    private static readonly double animationSlide = 15;

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      PART_CloseButton = GetTemplateChild(PART_CloseButton_Name) as Button;
      PART_TextBlock = GetTemplateChild(PART_TextBlock_Name) as TextBlock;

      PART_TextBlock.Text = Text;
      PART_CloseButton.Click += PART_CloseButton_ClickAsync;

      Left = screenPos.X;
      Top = screenPos.Y + PlacementTarget.ActualHeight + padding * 2;

      initialRect = new Rect(new Point(), mainPanel.RenderSize);
      rectGeo = new RectangleGeometry(initialRect, cornerRadius, cornerRadius);
      targetRect = new Rect(windowPos.Minus((padding, padding)), PlacementTarget.RenderSize.Add((padding, padding).Times(2)));
      var combinedGeo = new CombinedGeometry
      {
        GeometryCombineMode = GeometryCombineMode.Exclude,
        Geometry1 = new RectangleGeometry(initialRect),
        Geometry2 = rectGeo,
      };

      shadowBorder.Clip = combinedGeo;
      Panel.SetZIndex(shadowBorder, int.MaxValue);
      mainPanel.Children.Add(shadowBorder);
    }

    protected override void OnContentRendered(EventArgs e)
    {
      base.OnContentRendered(e);
      var content = Content as FrameworkElement;
      Width = content.Width + padding * 2;
      Height = content.Height + padding * 2;
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      base.OnMouseLeftButtonDown(e);
      this.DragMove();
    }

    protected override async void OnRender(DrawingContext drawingContext)
    {
      base.OnRender(drawingContext);
      this.ShiftWindowOntoScreen();
      rectGeo.BeginAnimation(RectangleGeometry.RectProperty, new RectAnimation(initialRect, targetRect, enterDuration));
      this.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, enterDuration));
      this.BeginAnimation(TopProperty, new DoubleAnimation(Top + animationSlide, Top, enterDuration));
      await NativeMethod.WaitAsync((obj) => false, null, enterMillisecond);

      this.BeginAnimation(TopProperty, null); //解除Animation對屬性的綁定
    }

    private async void PART_CloseButton_ClickAsync(object sender, RoutedEventArgs e)
    {
      this.BeginAnimation(OpacityProperty, new DoubleAnimation(1, 0, leaveDuration));
      this.BeginAnimation(TopProperty, new DoubleAnimation(Top, Top + animationSlide, leaveDuration));
      rectGeo.BeginAnimation(RectangleGeometry.RectProperty, new RectAnimation(targetRect, initialRect, leaveDuration));
      await NativeMethod.WaitAsync((obj) => false, null, leaveMillisecond);
      Close();
      mainPanel.Children.Remove(shadowBorder);
    }
  }
}
