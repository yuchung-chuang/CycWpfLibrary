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
using System.Windows.Shapes;

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
      mainWindow = Application.Current.MainWindow;
      mainWindow.EnsurePanelContent();
      mainPanel = mainWindow.Content as Panel;
      centerPanelPos = mainPanel.RenderSize.ToPoint().Divide(2);
      centerScreenPos = mainPanel.PointToScreenDPI(centerPanelPos);
      targetPanelPos = centerPanelPos;
      targetScreenPos = centerScreenPos;
      resources = new PopupWindowResources();
      resources.InitializeComponent();
      cornerRadius = (double)resources["cornerRadius"];
      shadow = new Border
      {
        Style = resources["shadowStyle"] as Style,
      };
      targetRectangle = new Rectangle
      {
        Style = resources["targetRectangleStyle"] as Style,
      };
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
      var target = popup.PlacementTarget;
      if (e.NewValue != null)
      {
        popup.targetPanelPos = target.TransformToAncestor(popup.mainPanel).Transform(new Point());
        popup.targetScreenPos = target.PointToScreenDPI(new Point());
      }
      else
      {
        popup.targetPanelPos = popup.centerPanelPos;
        popup.targetScreenPos = popup.centerScreenPos;
      }
    }
    #endregion

    private const string PART_TextBlock_Name = nameof(PART_TextBlock);
    private const string PART_CloseButton_Name = nameof(PART_CloseButton);

    private PopupWindowResources resources;
    private TextBlock PART_TextBlock;
    private Button PART_CloseButton;
    private Border shadow;
    private Rectangle targetRectangle;
    private Window mainWindow;
    private Panel mainPanel;

    private Point centerPanelPos;
    private Point targetPanelPos;
    private Point targetScreenPos;
    private Point centerScreenPos;
    private RectangleGeometry shadowRectGeo;
    private Rect initialRect;
    private Rect targetRect;

    private Thickness initialMargin;
    private Thickness targetMargin;
    private readonly double cornerRadius;
    private readonly double padding = 5d;
    private readonly double enterMs = 500;
    private readonly double leaveMs = 200;
    private readonly double animationSlide = 15;

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      PART_CloseButton = GetTemplateChild(PART_CloseButton_Name) as Button;
      PART_TextBlock = GetTemplateChild(PART_TextBlock_Name) as TextBlock;

      PART_TextBlock.Text = Text;
      PART_CloseButton.Click += PART_CloseButton_ClickAsync;

      initialRect = new Rect(new Point(), mainPanel.RenderSize);
      targetRect = new Rect(targetPanelPos.Minus((padding, padding)),
        PlacementTarget?.RenderSize.Add((padding, padding).Times(2)) ?? new Size());
      shadowRectGeo = new RectangleGeometry(PlacementTarget != null ? initialRect : targetRect,
        cornerRadius, cornerRadius);
      targetRectangle.Visibility = PlacementTarget != null ? Visibility.Visible : Visibility.Collapsed;
      var combinedGeo = new CombinedGeometry
      {
        GeometryCombineMode = GeometryCombineMode.Exclude,
        Geometry1 = new RectangleGeometry(initialRect),
        Geometry2 = shadowRectGeo,
      };
      shadow.Clip = combinedGeo;
      Panel.SetZIndex(shadow, int.MaxValue - 1);
      mainPanel.Children.Add(shadow);

      initialMargin = new Thickness(0, 0, 0, 0);
      targetMargin = new Thickness(targetRect.Left, targetRect.Top, 0, 0);
      targetRectangle.Width = initialRect.Width;
      targetRectangle.Height = initialRect.Height;
      targetRectangle.Margin = initialMargin;
      Panel.SetZIndex(targetRectangle, int.MaxValue);
      mainPanel.Children.Add(targetRectangle);
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      base.OnMouseLeftButtonDown(e);
      this.DragMove();
    }

    protected override async void OnRender(DrawingContext drawingContext)
    {
      base.OnRender(drawingContext);

      if (PlacementTarget != null)
      {
        Left = targetScreenPos.X;
        Top = targetScreenPos.Y + PlacementTarget.ActualHeight + padding * 2;
      }
      else
      {
        Left = centerScreenPos.X - ActualWidth / 2;
        Top = centerScreenPos.Y - ActualHeight / 2;
      }

      this.ShiftWindowOntoScreen();
      this.BeginAnimation(TopProperty, Top + animationSlide, Top, enterMs);
      this.BeginAnimation(OpacityProperty, 0d, 1, enterMs);
      if (PlacementTarget != null)
      {
        shadowRectGeo.BeginAnimation(RectangleGeometry.RectProperty, initialRect, targetRect, enterMs);
        targetRectangle.BeginAnimation(MarginProperty, targetMargin, enterMs);
        targetRectangle.BeginAnimation(WidthProperty, targetRect.Width, enterMs);
        targetRectangle.BeginAnimation(HeightProperty, targetRect.Height, enterMs);
      }
      shadow.BeginAnimation(OpacityProperty, 0d, 1, enterMs);

      await NativeMethod.WaitAsync((obj) => false, null, enterMs);
      this.BeginAnimation(TopProperty, null); //解除Animation對屬性的綁定
    }

    private async void PART_CloseButton_ClickAsync(object sender, RoutedEventArgs e)
    {
      this.BeginAnimation(OpacityProperty, 0d, leaveMs);
      this.BeginAnimation(TopProperty, Top + animationSlide, leaveMs);
      if (PlacementTarget != null)
      {
        shadowRectGeo.BeginAnimation(RectangleGeometry.RectProperty, initialRect, leaveMs);
        targetRectangle.BeginAnimation(MarginProperty, initialMargin, leaveMs);
        targetRectangle.BeginAnimation(WidthProperty, initialRect.Width, leaveMs);
        targetRectangle.BeginAnimation(HeightProperty, initialRect.Height, leaveMs);
      }
      shadow.BeginAnimation(OpacityProperty, 0d, leaveMs);

      await NativeMethod.WaitAsync((obj) => false, null, leaveMs);
      Close();
      mainPanel.Children.Remove(shadow);
      mainPanel.Children.Remove(targetRectangle);
    }
  }
}
