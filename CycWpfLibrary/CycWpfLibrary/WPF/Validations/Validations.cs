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
  public class NotNullValidation : ValidationRule
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.IsNotNull(value);
  }

  public class DoubleValidation : ValidationRule
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.IsDouble(value);
  }

  public class LogBaseValidation : ValidationRule
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.IsLogBase(value);
  }

  public class ByteValidation : ValidationRule
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.IsByte(value);
  }

  public class RangeValidation : ValidationRule
  {
    public int Maximum { get; set; } = int.MaxValue;
    public int Minimum { get; set; } = int.MinValue;

    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.IsInRange(value, Maximum, Minimum);
  }

  public class DateValidationRule : ValidationRule
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.IsDate(value);
  }

  public class FutureDateValidationRule : ValidationRule
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.IsFutureDate(value);
  }

  public class PastDateValidationRule : ValidationRule
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.IsPastDate(value);
  }

}
