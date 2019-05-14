using System;
using System.Windows;
using System.Windows.Markup;

namespace CycWpfLibrary
{
  [MarkupExtensionReturnType(typeof(Style))]
  public class MultiStyle : MarkupExtension
  {
    private string[] resourceKeys;

    public MultiStyle(string inputResourceKeys)
    {
      if (inputResourceKeys == null)
        throw new ArgumentNullException("inputResourceKeys");
      resourceKeys = inputResourceKeys.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
      if (resourceKeys.Length == 0)
        throw new ArgumentException("No input resource keys specified.");
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      var resultStyle = new Style();
      foreach (var currentResourceKey in resourceKeys)
      {
        object key = currentResourceKey;
        if (currentResourceKey == ".")
        {
          var service = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
          key = service.TargetObject.GetType();
        }
        var currentStyle = new StaticResourceExtension(key).ProvideValue(serviceProvider) as Style;
        if (currentStyle == null)
          throw new InvalidOperationException("Could not find style with resource key " + currentResourceKey + ".");
        resultStyle.Merge(currentStyle);
      }
      return resultStyle;
    }
  }
}
