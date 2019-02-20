using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
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
      resources = new PopupWindowResources();
      resources.InitializeComponent();
      cornerRadius = (double)resources["PopupCornerRadius"];
      shadow = new Border
      {
        Style = resources["shadowStyle"] as Style,
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

    public List<Inline> Inlines
    {
      get => (List<Inline>)GetValue(InlinesProperty);
      set => SetValue(InlinesProperty, value);
    }
    public static readonly DependencyProperty InlinesProperty = DependencyProperty.Register(
        nameof(Inlines),
        typeof(List<Inline>),
        typeof(PopupWindow),
        new PropertyMetadata(new List<Inline>()));

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
      if (e.NewValue != null)
        popup.PlacementTargets = new FrameworkElement[] { popup.PlacementTarget };
    }

    public FrameworkElement[] PlacementTargets
    {
      get => (FrameworkElement[])GetValue(PlacementTargetsProperty);
      set => SetValue(PlacementTargetsProperty, value);
    }
    public static readonly DependencyProperty PlacementTargetsProperty = DependencyProperty.Register(
        nameof(PlacementTargets),
        typeof(FrameworkElement[]),
        typeof(PopupWindow),
        new PropertyMetadata(default(FrameworkElement[]), OnPlacementTargetsChanged));

    private static void OnPlacementTargetsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var popup = d as PopupWindow;
      var target = popup.PlacementTargets;
      if (e.NewValue != null)
      {
        var n = target.Length;
        popup.targetPanelLoc = new Point[n];
        popup.targetScreenLoc = new Point[n];
        popup.targetRectangle = new Rectangle[n];
        popup.targetRect = new Rect[n];
        popup.targetMargin = new Thickness[n];
        popup.shadowRectGeo = new RectangleGeometry[n];
        for (int i = 0; i < target.Length; i++)
        {
          popup.targetPanelLoc[i] = target[i].TransformToAncestor(popup.mainPanel).Transform(new Point());
          popup.targetScreenLoc[i] = target[i].PointToScreenDPI(new Point());
          popup.targetRectangle[i] = new Rectangle
          {
            Style = popup.resources["targetRectangleStyle"] as Style,
          };
        }
      }
      else
      {
        throw new NotSupportedException();
      }
    }
    #endregion

    private const string PART_TextBlock_Name = nameof(PART_TextBlock);
    private const string PART_CloseButton_Name = nameof(PART_CloseButton);

    private PopupWindowResources resources;
    private TextBlock PART_TextBlock;
    private Button PART_CloseButton;
    private Border shadow;
    private Window mainWindow;
    private Panel mainPanel;
    private Point centerPanelPos;
    private Point centerScreenPos;

    private Rectangle[] targetRectangle;
    private Point[] targetPanelLoc;
    private Point[] targetScreenLoc;
    private RectangleGeometry[] shadowRectGeo;
    private Rect initialRect;
    private Rect[] targetRect;
    private Thickness initialMargin;
    private Thickness[] targetMargin;

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
      // add events
      PART_CloseButton.Click += PART_CloseButton_ClickAsync;
      // add Texts
      PART_TextBlock.Text = Text;
      foreach (var inline in Inlines)
      {
        PART_TextBlock.Inlines.Add(inline);
      }
      // define shapes
      initialRect = new Rect(new Point(), mainPanel.RenderSize);
      initialMargin = new Thickness(0, 0, 0, 0);
      for (int i = 0; i < PlacementTargets?.Length; i++)
      {
        targetRect[i] = new Rect(targetPanelLoc[i].Minus((padding, padding)), PlacementTargets[i]?.RenderSize.Add((padding, padding).Times(2)) ?? new Size());
        shadowRectGeo[i] = new RectangleGeometry(PlacementTargets.Length > 0 ? initialRect : targetRect[i], cornerRadius, cornerRadius);
        targetRectangle[i].Visibility = PlacementTargets.Length > 0 ? Visibility.Visible : Visibility.Collapsed;

        targetMargin[i] = new Thickness(targetRect[i].Left, targetRect[i].Top, 0, 0);
        targetRectangle[i].Width = initialRect.Width;
        targetRectangle[i].Height = initialRect.Height;
        targetRectangle[i].Margin = initialMargin;
        Panel.SetZIndex(targetRectangle[i], int.MaxValue);
        mainPanel.Children.Add(targetRectangle[i]);
      }

      CombinedGeometry combinedGeo = new CombinedGeometry
      {
        GeometryCombineMode = GeometryCombineMode.Exclude,
        Geometry1 = new RectangleGeometry(initialRect),
        Geometry2 = new RectangleGeometry(),
      };
      for (int i = 0; i < PlacementTargets?.Length; i++)
      {
        combinedGeo = new CombinedGeometry
        {
          GeometryCombineMode = GeometryCombineMode.Exclude,
          Geometry1 = combinedGeo,
          Geometry2 = shadowRectGeo[i],
        };
      }
      shadow.Clip = combinedGeo;
      Panel.SetZIndex(shadow, int.MaxValue - PlacementTargets?.Length ?? 0); //shadow underneath other controls
      mainPanel.Children.Add(shadow);
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      base.OnMouseLeftButtonDown(e);
      this.DragMove();
    }

    protected override async void OnRender(DrawingContext drawingContext)
    {
      base.OnRender(drawingContext);

      if (PlacementTargets?.Length == 1)
      {
        Left = targetScreenLoc[0].X;
        Top = targetScreenLoc[0].Y + PlacementTargets[0].ActualHeight + padding * 2;
      }
      else // if no target of more than one target, place popup in the center
      {
        Left = centerScreenPos.X - ActualWidth / 2;
        Top = centerScreenPos.Y - ActualHeight / 2;
      }

      this.ShiftWindowOntoScreen();
      this.BeginAnimation(TopProperty, Top + animationSlide, Top, enterMs);
      this.BeginAnimation(OpacityProperty, 0d, 1, enterMs);
      for (int i = 0; i < PlacementTargets?.Length; i++)
      {
        shadowRectGeo[i].BeginAnimation(RectangleGeometry.RectProperty, initialRect, targetRect[i], enterMs);
        targetRectangle[i].BeginAnimation(MarginProperty, targetMargin[i], enterMs);
        targetRectangle[i].BeginAnimation(WidthProperty, targetRect[i].Width, enterMs);
        targetRectangle[i].BeginAnimation(HeightProperty, targetRect[i].Height, enterMs);
      }
      shadow.BeginAnimation(OpacityProperty, 0d, 1, enterMs);

      await NativeMethod.WaitAsync((obj) => false, null, enterMs);
      this.BeginAnimation(TopProperty, null); //解除Animation對屬性的綁定
    }

    private async void PART_CloseButton_ClickAsync(object sender, RoutedEventArgs e)
    {
      this.BeginAnimation(OpacityProperty, 0d, leaveMs);
      this.BeginAnimation(TopProperty, Top + animationSlide, leaveMs);
      for (int i = 0; i < PlacementTargets?.Length; i++)
      {
        shadowRectGeo[i].BeginAnimation(RectangleGeometry.RectProperty, initialRect, leaveMs);
        targetRectangle[i].BeginAnimation(MarginProperty, initialMargin, leaveMs);
        targetRectangle[i].BeginAnimation(WidthProperty, initialRect.Width, leaveMs);
        targetRectangle[i].BeginAnimation(HeightProperty, initialRect.Height, leaveMs);
      }
      shadow.BeginAnimation(OpacityProperty, 0d, leaveMs);

      await NativeMethod.WaitAsync((obj) => false, null, leaveMs);
      Close();
      mainPanel.Children.Remove(shadow);
      mainPanel.Children.RemoveAll(targetRectangle);
    }
  }
}
