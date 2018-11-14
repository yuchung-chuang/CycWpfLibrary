using CycWpfLibrary.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycWpfLibrary
{
  public class PixelBitmapEditor : EditorBase<PixelBitmap>
  {
    public override bool CanEdit(object parameter) => true;

    private PixelBitmapEditor() { }
    public PixelBitmapEditor(PixelBitmap pixelBitmap) : base(pixelBitmap) { }
  }
}
