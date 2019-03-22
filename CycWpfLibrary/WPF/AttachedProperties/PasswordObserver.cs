using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CycWpfLibrary
{
  public static class PasswordObserver
  {
    #region Attached Properties
    public static readonly DependencyProperty ObservedPasswordProperty = DependencyProperty.RegisterAttached(
        "ObservedPassword",
        typeof(string),
        typeof(PasswordObserver),
        new FrameworkPropertyMetadata(string.Empty));
    [Category(AppNames.CycWpfLibrary)]
    [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
    public static string GetObservedPassword(DependencyObject obj)
        => (string)obj.GetValue(ObservedPasswordProperty);
    public static void SetObservedPassword(DependencyObject obj, string value)
        => obj.SetValue(ObservedPasswordProperty, value);

    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled",
        typeof(bool),
        typeof(PasswordObserver),
        new PropertyMetadata(false, OnIsEnabledChanged));
    [Category(AppNames.CycWpfLibrary)]
    [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
    public static bool GetIsEnabled(DependencyObject obj)
        => (bool)obj.GetValue(IsEnabledProperty);
    public static void SetIsEnabled(DependencyObject obj, bool value)
        => obj.SetValue(IsEnabledProperty, value);

    #endregion

    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var passwordBox = d as PasswordBox;

      if ((bool)e.NewValue)
      {
        passwordBox.PasswordChanged += OnPasswordChanged;
      }
      else
      {
        passwordBox.PasswordChanged -= OnPasswordChanged;
      }
    }

    private static void OnPasswordChanged(object sender, RoutedEventArgs e)
    {
      var passwordBox = sender as PasswordBox;
      SetObservedPassword(passwordBox, passwordBox.Password);
    }
  }
}
