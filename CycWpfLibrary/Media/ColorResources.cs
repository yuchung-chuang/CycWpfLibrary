using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CycWpfLibrary.Media
{
  public static class ColorResources
  {
    #region From BlurryControls

    /// <summary>
    /// Returns the current Windows accent color as <see cref="SolidColorBrush"/>
    /// </summary>
    public static SolidColorBrush SystemWindowGlassColorBrush => (SolidColorBrush)SystemParameters.WindowGlassBrush;
    /// <summary>
    /// Returns the current Windows accent color as <see cref="Brush"/>
    /// </summary>
    public static Brush SystemWindowGlassBrush => SystemParameters.WindowGlassBrush;
    /// <summary>
    /// Returns the current Windows accent color as <see cref="Color"/>
    /// </summary>
    public static Color SystemWindowGlassColor => SystemParameters.WindowGlassColor;

    /// <summary>
    /// Returns the inversion of  the current Windows accent color as <see cref="SolidColorBrush"/>
    /// </summary>
    public static SolidColorBrush InvertedSystemWindowGlassColorBrush => SystemParameters.WindowGlassBrush.Invert();
    /// <summary>
    /// Returns the inversion of the current Windows accent color as <see cref="Brush"/>
    /// </summary>
    public static Brush InvertedSystemWindowGlassBrush => SystemParameters.WindowGlassBrush.Invert();
    /// <summary>
    /// Returns the inversion of the current Windows accent color as <see cref="Color"/>
    /// </summary>
    public static Color InvertedSystemWindowGlassColor => SystemParameters.WindowGlassColor.Invert();

    /// <summary>
    /// Returns the current Windows accent color with a strength of 0.75 as <see cref="SolidColorBrush"/>
    /// </summary>
    public static SolidColorBrush TransparentSystemWindowGlassColorBrush => SystemParameters.WindowGlassBrush.OfStrength();
    /// <summary>
    /// Returns the current Windows accent color with a strength of 0.75 as <see cref="Brush"/>
    /// </summary>
    public static Brush TransparentSystemWindowGlassBrush => SystemParameters.WindowGlassBrush.OfStrength();
    /// <summary>
    /// Returns the current Windows accent color with a strength of 0.75 as <see cref="Color"/>
    /// </summary>
    public static Color TransparentSystemWindowGlassColor => SystemParameters.WindowGlassBrush.OfStrength().Color;

    /// <summary>
    /// Returns the inversion of the current Windows accent color with a strength of 0.75 as <see cref="SolidColorBrush"/>
    /// </summary>
    public static SolidColorBrush InvertedTransparentSystemWindowGlassColorBrush => SystemParameters.WindowGlassBrush.OfStrength().Invert();
    /// <summary>
    /// Returns the inversion of the current Windows accent color with a strength of 0.75 as <see cref="Brush"/>
    /// </summary>
    public static Brush InvertedTransparentSystemWindowGlassBrush => SystemParameters.WindowGlassBrush.OfStrength().Invert();
    /// <summary>
    /// Returns the inversion of the current Windows accent color with a strength of 0.75 as <see cref="Color"/>
    /// </summary>
    public static Color InvertedTransparentSystemWindowGlassColor => SystemParameters.WindowGlassBrush.OfStrength().Color.Invert();

    #endregion
  }
}
