using System.Windows;
using System.Windows.Forms;
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

    public RevealWindow()
    {
      this.notifyIcon = new NotifyIcon();
      this.notifyIcon.Icon = (this.Icon as BitmapSource).ToBitmap().ToIcon();
      this.notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
    }

    private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      this.Show();
      this.WindowState = WindowState.Normal;
    }
  }
}
