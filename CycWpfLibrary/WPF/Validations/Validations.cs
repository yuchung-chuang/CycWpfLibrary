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
  public class NotSimilarValidation : ValidationRuleBase
  {
    public MatchValidationDP DP { get; set; } = new MatchValidationDP();
    public double tolerance { get; set; }
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.NotSimilar(value, DP.Match, tolerance, Message);
  }

  public class ContainUpperValidation : ValidationRuleBase
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.ContainUpper(value, Message);
  }

  public class ContainLowerValidation : ValidationRuleBase
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.ContainLower(value, Message);
  }

  public class ContainSymbolValidation : ValidationRuleBase
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.ContainSymbol(value, Message);
  }
  public class MinLengthValidation : ValidationRuleBase
  {
    public int MinLength { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.MinLength(value, MinLength, Message);
  }

  public class MatchValidation : ValidationRuleBase
  {
    public MatchValidationDP DP { get; set; }
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.Match(value, DP.Match, Message);
  }

  public class MatchAnyValidation : ValidationRuleBase
  {
    public MatchListDP DP { get; set; } = new MatchListDP();
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.MatchAny(value, DP.MatchList, Message);
  }

  public class NoMatchValidation : ValidationRuleBase
  {
    public MatchListDP DP { get; set; } = new MatchListDP();

    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.NoMatch(value, DP.MatchList, Message);
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
