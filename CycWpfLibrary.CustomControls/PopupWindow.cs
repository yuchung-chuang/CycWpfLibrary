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
    private bool isDragMove;
    private Cursor cursorCache;

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      PART_CloseButton = GetTemplateChild(PART_CloseButton_Name) as Button;
      PART_TextBlock = GetTemplateChild(PART_TextBlock_Name) as TextBlock;

      PART_TextBlock.Text = Text;
      PART_CloseButton.Click += PART_CloseButton_Click;

      Left = screenPos.X;
      Top = screenPos.Y + PlacementTarget.ActualHeight + padding * 2;

      shadowBorder.Clip = new CombinedGeometry
      {
        GeometryCombineMode = GeometryCombineMode.Exclude,
        Geometry1 = new RectangleGeometry(new Rect(new Point(), mainPanel.RenderSize)),
        Geometry2 = new RectangleGeometry(new Rect(windowPos.Minus((padding, padding)), PlacementTarget.RenderSize.Add((padding, padding).Times(2))), cornerRadius, cornerRadius),
      };
      Panel.SetZIndex(shadowBorder, int.MaxValue);
      mainPanel.Children.Add(shadowBorder);
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
      base.OnRender(drawingContext);
      this.ShiftWindowOntoScreen();
    }

    protected override void OnContentRendered(EventArgs e)
    {
      base.OnContentRendered(e);
      var content = Content as FrameworkElement;
      Width = content.Width + padding * 2;
      Height = content.Height + padding * 2;
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
      base.OnMouseDown(e);
      if (e.ChangedButton == MouseButton.Left)
      {
        this.DragMove();
        isDragMove = true;
      }
    }
    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
      base.OnMouseUp(e);
      if (isDragMove)
      {
        isDragMove = false;
      }
    }

    private void PART_CloseButton_Click(object sender, RoutedEventArgs e)
    {
      mainPanel.Children.Remove(shadowBorder);
      Close();
    }
  }
}
