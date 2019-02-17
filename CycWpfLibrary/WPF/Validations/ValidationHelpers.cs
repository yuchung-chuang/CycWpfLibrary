using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CycWpfLibrary
{
  public static class ValidationHelpers
  {
    private static Application app = Application.Current;

    public static ValidationResult IsNotNull(object value)
    {
      return new ValidationResult(!string.IsNullOrWhiteSpace((value ?? "").ToString()), app.FindResource("ValidationMessageNotNull"));
    }

    public static ValidationResult IsDouble(object value)
    {
      if (string.IsNullOrEmpty((value ?? "").ToString()))
        return ValidationResult.ValidResult; // allows value to be null
      return new ValidationResult(double.TryParse(value.ToString(), out var num), app.FindResource("ValidationMessageDouble"));
    }

    public static ValidationResult IsLogBase(object value)
    {
      if (string.IsNullOrEmpty((value ?? "").ToString()))
        return ValidationResult.ValidResult;
      var doubleValidation = IsDouble(value);
      if (!doubleValidation.IsValid)
        return new ValidationResult(false, doubleValidation.ErrorContent);

      var logBase = value.ToString().ToDouble();
      return new ValidationResult(logBase > 0, app.FindResource("ValidationMessageLogBase"));
    }

    public static ValidationResult IsByte(object value)
    {
      if (string.IsNullOrEmpty((value ?? "").ToString()))
        return ValidationResult.ValidResult;
      var doubleValidation = IsDouble(value);
      if (!doubleValidation.IsValid)
        return new ValidationResult(false, doubleValidation.ErrorContent);

      var @byte = value.ToString().ToDouble();
      return new ValidationResult(Math.IsIn(@byte, 255, 0), app.FindResource("Byte must between (0, 255)"));
    }

    public static ValidationResult IsInRange(object value, int max, int min, bool excludeMax, bool excludeMin)
    {
      if (string.IsNullOrEmpty((value ?? "").ToString()))
        return ValidationResult.ValidResult;
      var doubleValidation = IsDouble(value);
      if (!doubleValidation.IsValid)
        return new ValidationResult(false, doubleValidation.ErrorContent);

      var num = value.ToString().ToDouble();
      return new ValidationResult(Math.IsIn(num, max, min, excludeMax, excludeMin), 
        app.FindResource("ValidationMessageInRange") +
        $"{(excludeMin ? "[" : "(")}" +
        $"{(min == int.MinValue ? "-∞" : min.ToString())}, " +
        $"{(max == int.MaxValue ? "∞" : max.ToString())}" +
        $"{(excludeMax ? "]" : ")")}");
    }

    public static ValidationResult IsDate(object value)
    {
      if (string.IsNullOrEmpty((value ?? "").ToString()))
        return ValidationResult.ValidResult;
      return new ValidationResult(DateTime.TryParse((value ?? "").ToString(), CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces, out var time), app.FindResource("ValidationMessageDate"));
    }

    public static ValidationResult IsFutureDate(object value)
    {
      if (string.IsNullOrEmpty((value ?? "").ToString()))
        return ValidationResult.ValidResult;
      var isDateValidation = IsDate(value);
      if (!isDateValidation.IsValid)
        return new ValidationResult(false, isDateValidation.ErrorContent);

      var time = value.ToString().ToDate();
      return new ValidationResult(time.Date > DateTime.Now.Date, app.FindResource("ValidationMessageFutureDate"));
    }

    public static ValidationResult IsPastDate(object value)
    {
      if (string.IsNullOrEmpty((value ?? "").ToString()))
        return ValidationResult.ValidResult;
      var isDateValidation = IsDate(value);
      if (!isDateValidation.IsValid)
        return new ValidationResult(false, isDateValidation.ErrorContent);

      var time = value.ToString().ToDate();
      return new ValidationResult(time.Date < DateTime.Now.Date, app.FindResource("ValidationMessagePastDate"));
    }

    public static bool IsValid(DependencyObject obj)
    {
      // The dependency object is valid if it has no errors and all
      // of its children (that are dependency objects) are error-free.
      return !Validation.GetHasError(obj) &&
        LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>().All(IsValid);
    }
  }
}
