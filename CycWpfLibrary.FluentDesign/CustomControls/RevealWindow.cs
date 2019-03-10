using System;
using System.Windows;
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

    private NotifyIcon notifyIcon;
    public ICommand NotifyIconCommand { get; set; }

    public RevealWindow()
    {
      this.notifyIcon = new NotifyIcon();
      this.Loaded += RevealWindow_Loaded;
      this.notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
      NotifyIconCommand = new RelayCommand(MinimizeToNotifyIcon);
    }

    private void RevealWindow_Loaded(object sender, RoutedEventArgs e)
    {
      this.notifyIcon.Icon = (this.Icon as BitmapSource).ToBitmap().ToIcon();
    }

    private void MinimizeToNotifyIcon()
    {
      this.Hide();
      notifyIcon.Visible = true;
    }

    private void NotifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
    {
      this.Show();
      this.WindowState = WindowState.Normal;
      notifyIcon.Visible = false;
    }
  }
}
