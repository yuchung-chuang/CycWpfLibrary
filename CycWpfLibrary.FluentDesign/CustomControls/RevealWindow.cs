using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CycWpfLibrary.FluentDesign
{
  public class RevealWindow : CustomWindowBase
  {
    static RevealWindow()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(RevealWindow), new FrameworkPropertyMetadata(typeof(RevealWindow)));
    }

    public RevealWindow()
    {
      InitializeNotifyIcon();
    }

    private void RevealWindow_Loaded(object sender, RoutedEventArgs e)
    {
      SetNotifyIcon();
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();

      AddCustomWindowControls();
    }


    #region TopmostButton
    [Browsable(true)]
    [Category(AppNames.CycWpfLibrary)]
    [Description("Showing Topmost Button in title bar")]
    public bool EnableTopmostButton { get; set; } = true; 
    #endregion

    #region CustomWindowControls
    public Collection<System.Windows.Controls.Control> CustomWindowControls { get; set; } = new Collection<System.Windows.Controls.Control>();
    private StackPanel WindowButtonsStackPanel;
    private void AddCustomWindowControls()
    {
      WindowButtonsStackPanel = GetTemplateChild(nameof(WindowButtonsStackPanel)) as StackPanel;
      foreach (var control in CustomWindowControls)
      {
        WindowButtonsStackPanel.Children.Add(control);
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
      Loaded += RevealWindow_Loaded;
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
  }
}
