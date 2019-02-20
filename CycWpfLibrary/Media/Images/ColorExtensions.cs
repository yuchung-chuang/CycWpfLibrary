using System;
using System.Windows;
using System.Windows.Media;
using static CycWpfLibrary.Math;

namespace CycWpfLibrary
{
  public static class ColorExtensions
  {
    /// <summary>
    /// 對<see cref="Color"/>做線性內插。
    /// </summary>
    /// <param name="StartColor">起始顏色。</param>
    /// <param name="EndColor">結束顏色。</param>
    /// <param name="Ratio">內插比率</param>
    /// <returns></returns>
    public static Color Interpolate(this Color StartColor, Color EndColor, float Ratio)
    {
      var A = (byte)Math.Interpolate(StartColor.A, EndColor.A, Ratio);
      var R = (byte)Math.Interpolate(StartColor.R, EndColor.R, Ratio);
      var G = (byte)Math.Interpolate(StartColor.G, EndColor.G, Ratio);
      var B = (byte)Math.Interpolate(StartColor.B, EndColor.B, Ratio);
      return Color.FromArgb(A, R, G, B);
    }

    /// <summary>
    /// 漂白<see cref="Color"/>。
    /// </summary>
    /// <param name="color">要漂白的顏色。</param>
    /// <param name="value">以256色為單位，加入<paramref name="color"/>的數值。</param>
    /// <returns></returns>
    public static Color WriteOut(this Color color, int value)
    {
      var R = (byte)Clamp(color.R + value, 255, 0);
      var G = (byte)Clamp(color.G + value, 255, 0);
      var B = (byte)Clamp(color.B + value, 255, 0);
      return Color.FromArgb(color.A, R, G, B);
    }

    public static SolidColorBrush ToBrush(this string HexColorString)
    {
      return (SolidColorBrush)new BrushConverter().ConvertFrom(HexColorString);
    }

    public static string ToHexString(this Color color)
    {
      var bytes = new byte[] { color.A, color.R, color.G, color.B };
      return bytes.ToHexString();
    }

    public static string ToHexString(this byte[] ba)
    {
      return BitConverter.ToString(ba).Replace("-", "");
    }
    

    #region From BlurryControls

    /// <summary>
    /// returns a <see cref="SolidColorBrush"/> of the given <see cref="SolidColorBrush"/> with inverted color channels
    /// </summary>
    /// <param name="colorBrush"></param>
    /// <returns>a <see cref="SolidColorBrush"/> with inverted color channels</returns>
    public static SolidColorBrush Invert(this SolidColorBrush colorBrush)
    {
      var color = colorBrush.Color;
      color.R = (byte)(255 - color.R);
      color.G = (byte)(255 - color.G);
      color.B = (byte)(255 - color.B);
      return new SolidColorBrush(color);
    }

    /// <summary>
    /// returns a <see cref="SolidColorBrush"/> of the given <see cref="Brush"/> with inverted color channels
    /// </summary>
    /// <param name="colorBrush"></param>
    /// <returns>a <see cref="SolidColorBrush"/> with inverted color channels</returns>
    public static SolidColorBrush Invert(this Brush colorBrush)
    {
      return ((SolidColorBrush)colorBrush).Invert();
    }

    /// <summary>
    /// returns a <see cref="Color"/> of the given <see cref="Color"/> with inverted color channels
    /// </summary>
    /// <param name="color"></param>
    /// <returns>a <see cref="Color"/> with inverted color channels</returns>
    public static Color Invert(this Color color)
    {
      color.R = (byte)(255 - color.R);
      color.G = (byte)(255 - color.G);
      color.B = (byte)(255 - color.B);
      return color;
    }


    /// <summary>
    /// returns a <see cref="SolidColorBrush"/> of the given <see cref="SolidColorBrush"/> with chosen opacity
    /// </summary>
    /// <param name="brush"></param>
    /// <param name="strength">opacity weight from 0.0 to 1.0 and is set to 0.75 if no value is provided</param>
    /// <returns>a given <see cref="SolidColorBrush"/> with chosen opacity</returns>
    public static SolidColorBrush OfStrength(this SolidColorBrush brush, double strength = 0.75d)
    {
      if (strength < 0d || strength > 1.0d)
        throw new ArgumentOutOfRangeException(nameof(strength), @"strength must be a value between 0.0 and 1.0");
      return brush.OfStrength((byte)(strength * 255));
    }

    /// <summary>
    /// returns a <see cref="SolidColorBrush"/> of the given <see cref="Color"/> with chosen opacity
    /// </summary>
    /// <param name="color"></param>
    /// <param name="strength">opacity weight from 0.0 to 1.0 and is set to 0.75 if no value is provided</param>
    /// <returns>a given <see cref="SolidColorBrush"/> with chosen opacity</returns>
    public static SolidColorBrush OfStrength(this Color color, double strength = 0.75d)
    {
      if (strength < 0d || strength > 1d)
        throw new ArgumentOutOfRangeException(nameof(strength), @"strength must be a value between 0.0 and 1.0");
      return OfStrength(new SolidColorBrush(color), (byte)(strength * 255));
    }

    /// <summary>
    /// returns a <see cref="SolidColorBrush"/> of the given <see cref="Brush"/> with chosen opacity
    /// </summary>
    /// <param name="brush"></param>
    /// <param name="strength">opacity weight from 0.0 to 1.0 and is set to 0.75 if no value is provided</param>
    /// <returns>a given <see cref="SolidColorBrush"/> with chosen opacity</returns>
    public static SolidColorBrush OfStrength(this Brush brush, double strength = 0.75d)
    {
      if (strength < 0d || strength > 1d)
        throw new ArgumentOutOfRangeException(nameof(strength), @"strength must be a value between 0.0 and 1.0");
      return ((SolidColorBrush)brush).OfStrength((byte)(strength * 255));
    }

    /// <summary>
    /// returns a <see cref="SolidColorBrush"/> of the given <see cref="SolidColorBrush"/> with chosen opacity
    /// </summary>
    /// <param name="colorBrush"></param>
    /// <param name="strength">opacity weight from 0 to 255 and is set to 191 if no value is provided</param>
    /// <returns>a given <see cref="SolidColorBrush"/> with chosen opacity</returns>
    public static SolidColorBrush OfStrength(this SolidColorBrush colorBrush, byte strength = 191)
    {
      var color = colorBrush.Color;
      color.A = strength == 0 ? (byte)1 : strength; //minimum 1 for input handling
      return new SolidColorBrush(color);
    }

    /// <summary>
    /// returns the currently selected <see cref="SystemParameters.WindowGlassColor"/> with chosen opacity
    /// </summary>
    /// <param name="strength">opacity weight from 0.0 to 1.0 and is set to 0.75 if no value is provided</param>
    /// <returns>the currently selected <see cref="SystemParameters.WindowGlassColor"/> with chosen opacity</returns>
    public static SolidColorBrush SystemWindowGlassBrushOfStrength(double strength = 0.75)
    {
      return SystemParameters.WindowGlassBrush.OfStrength(strength);
    }

    /// <summary>
    /// returns the <see cref="Color"/> of a given <see cref="Brush"/>
    /// </summary>
    /// <param name="brush"></param>
    /// <returns>the <see cref="Color"/> of a given <see cref="Brush"/></returns>
    public static Color GetColor(this Brush brush)
    {
      return ((SolidColorBrush)brush).Color;
    }

    /// <summary>
    /// returns a <see cref="SolidColorBrush"/> from <see cref="Color"/>
    /// </summary>
    /// <param name="color"></param>
    /// <returns>a <see cref="SolidColorBrush"/> with the given <see cref="Color"/></returns>
    public static SolidColorBrush GetBrush(this Color color)
    {
      return new SolidColorBrush(color);
    }

    #endregion 
  }
}
