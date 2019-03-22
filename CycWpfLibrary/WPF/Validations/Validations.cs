using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace CycWpfLibrary
{
  public class NoMatchValidation : ValidationRuleBase
  {
    public List<string> StringList { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => new ValidationResult(!StringList.Any(str => str == value.ToStringEx()), Message);
  }

  public class NotNullValidation : ValidationRuleBase
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.IsNotNull(value, Message);
  }

  public class DoubleValidation : ValidationRuleBase
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.IsDouble(value);
  }

  public class LogBaseValidation : ValidationRuleBase
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.IsLogBase(value);
  }

  public class ByteValidation : ValidationRuleBase
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.IsByte(value);
  }

  public class RangeValidation : ValidationRuleBase
  {
    public int Maximum { get; set; } = int.MaxValue;
    public int Minimum { get; set; } = int.MinValue;
    public bool ExcludeMax { get; set; }
    public bool ExcludeMin { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.IsInRange(value, Maximum, Minimum, ExcludeMax, ExcludeMin);
  }

  public class DateValidation : ValidationRuleBase
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.IsDate(value);
  }

  public class FutureDateValidation : ValidationRuleBase
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.IsFutureDate(value);
  }

  public class PastDateValidation : ValidationRuleBase
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.IsPastDate(value);
  }

}
