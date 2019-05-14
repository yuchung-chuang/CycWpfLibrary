using System.Globalization;
using System.Windows.Controls;

namespace CycWpfLibrary
{
  public class NotSimilarValidation : ValidationRuleBase
  {
    public MatchValidationDP DP { get; set; } = new MatchValidationDP();
    public double tolerance { get; set; }
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.NotSimilar(value, DP.Match, tolerance, Message);
  }

  public class HasNumberValidation : ValidationRuleBase
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.HasNumber(value, Message);
  }

  public class HasUpperValidation : ValidationRuleBase
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.HasUpper(value, Message);
  }

  public class HasLowerValidation : ValidationRuleBase
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.HasLower(value, Message);
  }

  public class HasSymbolValidation : ValidationRuleBase
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.HasSymbol(value, Message);
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
