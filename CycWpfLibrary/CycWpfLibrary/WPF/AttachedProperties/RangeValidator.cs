using CycWpfLibrary.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CycWpfLibrary
{
  public static class RangeValidator
  {
    #region Attached Properties
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled",
        typeof(bool),
        typeof(RangeValidator),
        new PropertyMetadata(default(bool), OnIsEnabledChanged));
    [Category(AppNames.CycWpfLibrary)]
    [AttachedPropertyBrowsableForType(typeof(TextBox))]
    public static bool GetIsEnabled(DependencyObject obj)
        => (bool)obj.GetValue(IsEnabledProperty);
    public static void SetIsEnabled(DependencyObject obj, bool value)
        => obj.SetValue(IsEnabledProperty, value);

    public static readonly DependencyProperty MinimumProperty = DependencyProperty.RegisterAttached(
        "Minimum",
        typeof(int),
        typeof(RangeValidator),
        new PropertyMetadata(int.MinValue));
    [Category(AppNames.CycWpfLibrary)]
    [AttachedPropertyBrowsableForType(typeof(TextBox))]
    public static int GetMinimum(DependencyObject obj)
        => (int)obj.GetValue(MinimumProperty);
    public static void SetMinimum(DependencyObject obj, int value)
        => obj.SetValue(MinimumProperty, value);

    public static readonly DependencyProperty MaximumProperty = DependencyProperty.RegisterAttached(
        "Maximum",
        typeof(int),
        typeof(RangeValidator),
        new PropertyMetadata(int.MaxValue));
    [Category(AppNames.CycWpfLibrary)]
    [AttachedPropertyBrowsableForType(typeof(TextBox))]
    public static int GetMaximum(DependencyObject obj)
        => (int)obj.GetValue(MaximumProperty);
    public static void SetMaximum(DependencyObject obj, int value)
        => obj.SetValue(MaximumProperty, value);

    public static readonly DependencyProperty ExcludeMaxProperty = DependencyProperty.RegisterAttached(
        "ExcludeMax",
        typeof(bool),
        typeof(RangeValidator),
        new PropertyMetadata(default(bool)));
    [Category(AppNames.CycWpfLibrary)]
    [AttachedPropertyBrowsableForType(typeof(TextBox))]
    public static bool GetExcludeMax(DependencyObject obj)
        => (bool)obj.GetValue(ExcludeMaxProperty);
    public static void SetExcludeMax(DependencyObject obj, bool value)
        => obj.SetValue(ExcludeMaxProperty, value);

    public static readonly DependencyProperty ExcludeMinProperty = DependencyProperty.RegisterAttached(
        "ExcludeMin",
        typeof(bool),
        typeof(RangeValidator),
        new PropertyMetadata(default(bool)));
    [Category(AppNames.CycWpfLibrary)]
    [AttachedPropertyBrowsableForType(typeof(TextBox))]
    public static bool GetExcludeMin(DependencyObject obj)
        => (bool)obj.GetValue(ExcludeMinProperty);
    public static void SetExcludeMin(DependencyObject obj, bool value)
        => obj.SetValue(ExcludeMinProperty, value);
    #endregion

    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (!(d is TextBox textBox))
        throw new NotSupportedException($"Can only set the {IsEnabledProperty} attached behavior on a {typeof(TextBox)}.");

      if ((bool)e.NewValue)
      {
        textBox.TextChanged += TextBox_TextChanged;
      }
      else
      {
        textBox.TextChanged -= TextBox_TextChanged;
      }
    }

    private static void TextBox_TextChanged(object sender, EventArgs e)
    {
      Validate(sender as TextBox);
    }

    private static void Validate(TextBox tb)
    {
      BindingExpression exp = tb.GetBindingExpression(TextBox.TextProperty);
      if (exp != null && exp.ParentBinding != null)
      {
        var myRule = exp.ParentBinding.ValidationRules.FirstOrDefault(r => r is RangeValidation) as RangeValidation;
        if (myRule == null)
        {
          myRule = new RangeValidation();
          exp.ParentBinding.ValidationRules.Add(myRule);
        }
        myRule.ValidatesOnTargetUpdated = true;
        myRule.Minimum = (int)tb.GetValue(MinimumProperty);
        myRule.Maximum = (int)tb.GetValue(MaximumProperty);
        myRule.ExcludeMax = (bool)tb.GetValue(ExcludeMaxProperty);
        myRule.ExcludeMin = (bool)tb.GetValue(ExcludeMinProperty);
        myRule.Validate(tb.Text, CultureInfo.CurrentCulture);
      }
    }
  }
}
