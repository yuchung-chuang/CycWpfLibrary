using Emgu.CV.OCR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CycWpfLibrary.Emgu
{
  //public class Obsolete
  //{
  //  public double? XMaxT { get; set; }
  //  public double? XMinT { get; set; }
  //  public double? XLogT { get; set; }
  //  public double? YMaxR { get; set; }
  //  public double? YMinR { get; set; }
  //  public double? YLogR { get; set; }

  //  [Obsolete]
  //  public PixelBitmap PBModified { get; set; }
  //  [Obsolete]
  //  public BitmapSource ImageSource => PBModified?.ToBitmapSource();
  //  [Obsolete]
  //  private readonly Tesseract ocr = IP.InitializeOcr("", "eng", OcrEngineMode.TesseractOnly, "0123456789");
  //  private AxisType AxisType => appData.AxisType;
  //  private Rect Axis => appData.Axis;
  //  private (double width, double height) ocrLength => (Axis.Width / 5, Axis.Height / 5);
  //  private Tesseract.Character[] GetLocalAxisLimit(Image<Bgr, byte> image, Rectangle rectangle)
  //  {
  //    image.ROI = rectangle;
  //    var ocredText = IP.OcrImage(ocr, image);
  //    IP.DrawCharacters(image, ocredText);
  //    image.ClearROI();
  //    CvInvoke.Rectangle(image, rectangle, Colors.Red.ToMCvScalar());
  //    return ocredText;
  //  }
  //  public ICommand GetAxisLimitCommand { get; set; }
  //  [Obsolete]
  //  public void GetAxisLimit()
  //  {

  //    var image = new Image<Bgr, byte>(PBInput.Bitmap);
  //    Mat imgGrey = new Mat();
  //    CvInvoke.CvtColor(image, imgGrey, ColorConversion.Bgr2Gray);
  //    Mat imgThresholded = new Mat();
  //    CvInvoke.Threshold(imgGrey, imgThresholded, 190, 255, ThresholdType.Binary);
  //    image = imgThresholded.ToImage<Bgr, byte>();

  //    if (AxisType.Left)
  //    {
  //      var rectangleLT = new Rect(Axis.Left - ocrLength.width / 2, Axis.Top - ocrLength.height / 2, ocrLength.width / 2, ocrLength.height).ToWinForm();
  //      var rectangleLB = new Rect(Axis.Left - ocrLength.width / 2, Axis.Bottom - ocrLength.height / 2, ocrLength.width / 2, ocrLength.height).ToWinForm();
  //      var charLT = GetLocalAxisLimit(image, rectangleLT);
  //      var charLB = GetLocalAxisLimit(image, rectangleLB);

  //      //var distances = charLT.Select(c => Point.Subtract(c.Region.Location.ToWpf(), Axis.Location).Length);
  //      //var minDistance = distances.Min();
  //      //var minIndex = distances.ToList().IndexOf(minDistance);
  //      // Align the text and find the nearest one!!
  //    }
  //    if (AxisType.Bottom)
  //    {
  //      var rectangleBL = new Rect(Axis.Left - ocrLength.width / 2, Axis.Bottom, ocrLength.width, ocrLength.height / 2).ToWinForm();
  //      var rectangleBR = new Rect(Axis.Right - ocrLength.width / 2, Axis.Bottom, ocrLength.width, ocrLength.height / 2).ToWinForm();
  //      var charBL = GetLocalAxisLimit(image, rectangleBL);
  //      var charBR = GetLocalAxisLimit(image, rectangleBR);

  //    }
  //    if (AxisType.Right)
  //    {
  //      var rectangleRB = new Rect(Axis.Right, Axis.Bottom - ocrLength.height / 2, ocrLength.width / 2, ocrLength.height).ToWinForm();
  //      var rectangleRT = new Rect(Axis.Right, Axis.Top - ocrLength.height / 2, ocrLength.width / 2, ocrLength.height).ToWinForm();
  //      var charRB = GetLocalAxisLimit(image, rectangleRB);
  //      var charRT = GetLocalAxisLimit(image, rectangleRT);
  //    }
  //    if (AxisType.Top)
  //    {
  //      var rectangleTR = new Rect(Axis.Right - ocrLength.width / 2, Axis.Top - ocrLength.height / 2, ocrLength.width, ocrLength.height / 2).ToWinForm();
  //      var rectangleTL = new Rect(Axis.Left - ocrLength.width / 2, Axis.Top - ocrLength.height / 2, ocrLength.width, ocrLength.height / 2).ToWinForm();
  //      var charTR = GetLocalAxisLimit(image, rectangleTR);
  //      var charTL = GetLocalAxisLimit(image, rectangleTL);
  //    }

  //    PBModified = image.Bitmap.ToPixelBitmap();
  //  }
  //}
}
