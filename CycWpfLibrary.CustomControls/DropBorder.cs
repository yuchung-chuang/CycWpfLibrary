using System;
using System.Collections.Generic;
using System.IO;
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

namespace CycWpfLibrary.CustomControls
{
  /// <summary>
  /// A <see cref="Border"/> that can receive dropped items.
  /// </summary>
  public class DropBorder : ContentControl
  {
    static DropBorder()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(DropBorder), new FrameworkPropertyMetadata(typeof(DropBorder)));
    }

    #region DPs
    /// <summary>
    /// Get the first dropped file name.
    /// </summary>
    public string DropFileName
    {
      get => (string)GetValue(DropFileNameProperty);
      set => SetValue(DropFileNameProperty, value);
    }
    public static readonly DependencyProperty DropFileNameProperty = DependencyProperty.Register(
      nameof(DropFileName),
      typeof(string),
      typeof(DropBorder));

    /// <summary>
    /// Get all dropped file names.
    /// </summary>
    public string[] DropFileNames
    {
      get => (string[])GetValue(DropFileNamesProperty);
      set => SetValue(DropFileNamesProperty, value);
    }
    public static readonly DependencyProperty DropFileNamesProperty = DependencyProperty.Register(
      nameof(DropFileNames),
      typeof(string[]),
      typeof(DropBorder));

    #endregion

    public bool AllowMultipleDrop { get; set; }

    private const string PART_Border_Name = nameof(PART_Border);
    private Border PART_Border;

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      PART_Border = GetTemplateChild(PART_Border_Name) as Border;
      PART_Border.Drop += PART_Border_Drop;
    }

    private void PART_Border_Drop(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(DataFormats.FileDrop))
        return;

      // Note that you can have more than one file.
      string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

      DropFileName = files[0];
      DropFileNames = AllowMultipleDrop ? files : files.Take(1).ToArray();
    }
  }
}
