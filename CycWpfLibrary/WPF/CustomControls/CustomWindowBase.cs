using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace CycWpfLibrary
{
  public abstract class CustomWindowBase : ObservableWindow
  {
    public CustomWindowBase()
    {
      SetSystemCommandBinding();
      InitializeNotifyIcon();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      SetNotifyIcon();
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      base.OnMouseLeftButtonDown(e);
      
    }
    protected override void OnContentRendered(EventArgs e)
    {
      base.OnContentRendered(e);
      if (SizeToContent == SizeToContent.WidthAndHeight)
        InvalidateMeasure();
    }
    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();

      AddCustomWindowControls();
    }

    #region Win32 Messages
    private const int WM_MOUSEHWHEEL = 0x020E;
    protected override void OnSourceInitialized(EventArgs e)
    {
      base.OnSourceInitialized(e);
      var source = PresentationSource.FromVisual(this) as HwndSource;
      source?.AddHook(WndProc);
    }

    /// <summary>
    /// Processing Win32 messages.
    /// </summary>
    /// <param name="hwnd">Window handle</param>
    /// <param name="msg">Message ID</param>
    /// <param name="wParam">Message "w" pointer.</param>
    /// <param name="lParam">Message "l" pointer.</param>
    /// <returns>Return specific value for specific message. Check documents of Win32 message processing.</returns>
    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
      switch (msg)
      {
        case WM_MOUSEHWHEEL:
          int tilt = (short)wParam.GetHIWORD();
          HookMouseTilt(tilt);
          return (IntPtr)1;
      }
      return IntPtr.Zero;
    }
    #endregion

    #region MouseTilt
    /// <summary>
    /// Invoked by Win32 message.
    /// </summary>
    private void HookMouseTilt(int tilt)
    {
      OnMouseTilt(tilt); // invoke event
    }
    public event EventHandler<MouseTiltEventArgs> MouseTilt;
    /// <summary>
    /// Invoked by <see cref="HookMouseTilt(int)"/>
    /// </summary>
    protected virtual void OnMouseTilt(int tilt)
    {
      MouseTilt?.Invoke(this, new MouseTiltEventArgs(tilt));
    }
    #endregion

    #region TitlebarControls
    public Collection<System.Windows.Controls.Control> TitlebarControls { get; set; } = new Collection<System.Windows.Controls.Control>();
    private StackPanel TitlebarControlsStackPanel;
    private void AddCustomWindowControls()
    {
      TitlebarControlsStackPanel = GetTemplateChild(nameof(TitlebarControlsStackPanel)) as StackPanel;
      foreach (var control in TitlebarControls)
      {
        TitlebarControlsStackPanel.Children.Add(control);
      }
    }
    #endregion

    #region NotifyIcon
    [Browsable(true)]
    [Category(AppNames.CycWpfLibrary)]
    [Description("Showing NotifyIcon Button in the title bar")]
    public bool EnableNotifyIconButton { get; set; } = true;
    protected NotifyIcon NotifyIcon { get; set; }
    public ICommand NotifyIconCommand { get; set; }

    private void InitializeNotifyIcon()
    {
      NotifyIcon = new NotifyIcon();
      Loaded += Window_Loaded;
      NotifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
      NotifyIconCommand = new RelayCommand(MinimizeToNotifyIcon);
    }
    private void SetNotifyIcon()
    {
      NotifyIcon.Icon = (Icon as BitmapSource).ToBitmap().ToIcon();
    }
    private void MinimizeToNotifyIcon()
    {
      Hide();
      NotifyIcon.Visible = true;
    }
    private void NotifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
    {
      Show();
      WindowState = WindowState.Normal;
      Activate();
    }
    #endregion

    #region TopmostButton
    [Browsable(true)]
    [Category(AppNames.CycWpfLibrary)]
    [Description("Showing Topmost Button in title bar")]
    public bool EnableTopmostButton { get; set; } = true;
    #endregion

    #region SystemCommands
    private void SetSystemCommandBinding()
    {
      CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
      CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeWindow, CanResizeWindow));
      CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow, CanMinimizeWindow));
      CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, RestoreWindow, CanResizeWindow));
      CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, ShowSystemMenu));
    }
    private void CanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;
    }
    private void CanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = ResizeMode != ResizeMode.NoResize;
    }
    private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
    {
      this.Close();
    }
    private void MaximizeWindow(object sender, ExecutedRoutedEventArgs e)
    {
      SystemCommands.MaximizeWindow(this);
    }
    private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e)
    {
      SystemCommands.MinimizeWindow(this);
    }
    private void RestoreWindow(object sender, ExecutedRoutedEventArgs e)
    {
      SystemCommands.RestoreWindow(this);
    }
    private void ShowSystemMenu(object sender, ExecutedRoutedEventArgs e)
    {
      var element = e.OriginalSource as FrameworkElement;
      if (element == null)
        return;

      var point = WindowState == WindowState.Maximized ? new Point(0, element.ActualHeight)
          : new Point(Left + BorderThickness.Left, element.ActualHeight + Top + BorderThickness.Top);
      point = element.TransformToAncestor(this).Transform(point);
      SystemCommands.ShowSystemMenu(this, point);
    }
    #endregion

    #region DragMove
    public bool EnableDragMove { get; set; }
    private void ProcessDragMove(MouseButtonEventArgs e)
    {
      if (EnableDragMove && e.ButtonState == MouseButtonState.Pressed)
        DragMove();
    } 
    #endregion
  }

  public class MouseTiltEventArgs : EventArgs
  {
    public int Tilt { get; set; }
    public MouseTiltEventArgs(int tilt)
    {
      Tilt = tilt;
    }
  }
}
