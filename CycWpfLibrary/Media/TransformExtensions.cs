using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CycWpfLibrary.Media
{
  public static class TransformExtensions
  {
    public static (TranslateTransform, ScaleTransform, RotateTransform, SkewTransform) SplitTransforms(this TransformCollection transforms) => (GetTranslate(transforms), 
      GetScale(transforms), 
      GetRotate(transforms), 
      GetSkew(transforms));

    public static TranslateTransform GetTranslate(this TransformCollection transforms) => transforms.FirstOrDefault(tr => tr is TranslateTransform) as TranslateTransform;
    public static ScaleTransform GetScale(this TransformCollection transforms) => transforms.FirstOrDefault(tr => tr is ScaleTransform) as ScaleTransform;
    public static RotateTransform GetRotate(this TransformCollection transforms) => transforms.FirstOrDefault(tr => tr is RotateTransform) as RotateTransform;
    public static SkewTransform GetSkew(this TransformCollection transforms) => transforms.FirstOrDefault(tr => tr is SkewTransform) as SkewTransform;

    public static int IndexOfTranslate(this TransformCollection transforms) => transforms.ToList().FindIndex(tr => tr is TranslateTransform);
    public static int IndexOfScale(this TransformCollection transforms) => transforms.ToList().FindIndex(tr => tr is ScaleTransform);
    public static int IndexOfRotate(this TransformCollection transforms) => transforms.ToList().FindIndex(tr => tr is RotateTransform);
    public static int IndexOfSkew(this TransformCollection transforms) => transforms.ToList().FindIndex(tr => tr is SkewTransform);

    public static (int, int, int, int) GetIndices(this TransformCollection transforms) => 
      (IndexOfTranslate(transforms),
      IndexOfScale(transforms),
      IndexOfRotate(transforms),
      IndexOfSkew(transforms));
  }
}
