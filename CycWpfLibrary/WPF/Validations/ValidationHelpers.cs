using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace CycWpfLibrary
{
  public static class ValidationHelpers
  {
    static ValidationHelpers()
    {
      messages_enUS.InitializeComponent();
    }

    #region Fields
    private static readonly Application app = Application.Current;
    private static readonly ValidationMessages_enUS messages_enUS = new ValidationMessages_enUS();

    private static readonly string NotSimilarMessage = "ValidationMessageNotSimilar";
    private static readonly string ContainUpperMessage = "ValidationMessageContainUpper";
    private static readonly string ContainLowerMessage = "ValidationMessageContainLower";
    private static readonly string ContainSymbolMessage = "ValidationMessageContainSymbol";
    private static readonly string MinLengthMessage = "ValidationMessageMinLength";
    private static readonly string MatchMessage = "ValidationMessageMatch";
    private static readonly string MatchAnyMessage = "ValidationMessageMatchAny";
    private static readonly string NoMatchMessage = "ValidationMessageNoMatch";
    private static readonly string IsNotNullMessage = "ValidationMessageNotNull";
    private static readonly string IsDoubleMessage = "ValidationMessageDouble";
    private static readonly string IsLogBaseMessage = "ValidationMessageLogBase";
    private static readonly string IsByteMessage = "ValidationMessageByte";
    private static readonly string IsInRangeMessage = "ValidationMessageRange";
    private static readonly string IsDateMessage = "ValidationMessageDate";
    private static readonly string IsFutureDateMessage = "ValidationMessageFutureDate";
    private static readonly string IsPastDateMessage = "ValidationMessagePastDate";
    #endregion

    public static ValidationResult NotSimilar(object value, string match, double tolerance, string message)
    {
      return new ValidationResult(value.ToStringEx().IsNotSimilarTo(match, tolerance), message ?? GetDefaultMessage(NotSimilarMessage) + match);
    }
    public static ValidationResult ContainUpper(object value, string message)
    {
      return new ValidationResult(value.ToStringEx().ContainUpper(), message ?? GetDefaultMessage(ContainUpperMessage));
    }
    public static ValidationResult ContainLower(object value, string message)
    {
      return new ValidationResult(value.ToStringEx().ContainLower(), message ?? GetDefaultMessage(ContainLowerMessage));
    }
    public static ValidationResult ContainSymbol(object value, string message)
    {
      return new ValidationResult(value.ToStringEx().ContainSymbol(), message ?? GetDefaultMessage(ContainSymbolMessage));
    }
    public static ValidationResult MinLength(object value, int minLength, string message)
    {
      return new ValidationResult(value.ToStringEx().Length >= minLength, message ?? GetDefaultMessage(MinLengthMessage) + minLength.ToString());
    }
    public static ValidationResult Match(object value, string match, string message)
    {
      return new ValidationResult(match == value.ToStringEx(), message ?? GetDefaultMessage(MatchMessage));
    }

    private static bool MatchAny(object value, List<string> matchList)
    {
      return matchList.Any(str => str == value.ToStringEx());
    }
    public static ValidationResult MatchAny(object value, List<string> matchList, string message)
    {
      return new ValidationResult(MatchAny(value, matchList), message ?? GetDefaultMessage(MatchAnyMessage));
    }
    public static ValidationResult NoMatch(object value, List<string> matchList, string message)
    {
      return new ValidationResult(!MatchAny(value, matchList), message ?? GetDefaultMessage(NoMatchMessage));
    }
    public static ValidationResult IsNotNull(object value, string message = null)
    {
      return new ValidationResult(!string.IsNullOrWhiteSpace(value.ToStringEx()), message ?? GetDefaultMessage(IsNotNullMessage));
    }
    public static ValidationResult IsDouble(object value)
    {
      if (string.IsNullOrEmpty(value.ToStringEx()))
        return ValidationResult.ValidResult; // allows value to be null
      return new ValidationResult(double.TryParse(value.ToString(), out var num), GetDefaultMessage(IsDoubleMessage));
    }
    public static ValidationResult IsLogBase(object value)
    {
      if (string.IsNullOrEmpty(value.ToStringEx()))
        return ValidationResult.ValidResult;
      var doubleValidation = IsDouble(value);
      if (!doubleValidation.IsValid)
        return new ValidationResult(false, doubleValidation.ErrorContent);

      var logBase = value.ToString().ToDouble();
      return new ValidationResult(logBase > 0, GetDefaultMessage(IsLogBaseMessage));
    }
    public static ValidationResult IsByte(object value)
    {
      if (string.IsNullOrEmpty(value.ToStringEx()))
        return ValidationResult.ValidResult;
      var doubleValidation = IsDouble(value);
      if (!doubleValidation.IsValid)
        return new ValidationResult(false, doubleValidation.ErrorContent);

      var @byte = value.ToString().ToDouble();
      return new ValidationResult(Math.IsIn(@byte, 255, 0), GetDefaultMessage(IsByteMessage));
    }
    public static ValidationResult IsInRange(object value, int max, int min, bool excludeMax, bool excludeMin)
    {
      if (string.IsNullOrEmpty(value.ToStringEx()))
        return ValidationResult.ValidResult;
      var doubleValidation = IsDouble(value);
      if (!doubleValidation.IsValid)
        return new ValidationResult(false, doubleValidation.ErrorContent);

      var num = value.ToString().ToDouble();
      return new ValidationResult(Math.IsIn(num, max, min, excludeMax, excludeMin),
        GetDefaultMessage(IsInRangeMessage) +
        $"{(excludeMin ? "[" : "(")}" +
        $"{(min == int.MinValue ? "-∞" : min.ToString())}, " +
        $"{(max == int.MaxValue ? "∞" : max.ToString())}" +
        $"{(excludeMax ? "]" : ")")}");
    }
    public static ValidationResult IsDate(object value)
    {
      if (string.IsNullOrEmpty(value.ToStringEx()))
        return ValidationResult.ValidResult;
      return new ValidationResult(DateTime.TryParse(value.ToStringEx(), CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces, out var time), GetDefaultMessage(IsDateMessage));
    }
    public static ValidationResult IsFutureDate(object value)
    {
      if (string.IsNullOrEmpty(value.ToStringEx()))
        return ValidationResult.ValidResult;
      var isDateValidation = IsDate(value);
      if (!isDateValidation.IsValid)
        return new ValidationResult(false, isDateValidation.ErrorContent);

      var time = value.ToString().ToDate();
      return new ValidationResult(time.Date > DateTime.Now.Date, GetDefaultMessage(IsFutureDateMessage));
    }
    public static ValidationResult IsPastDate(object value)
    {
      if (string.IsNullOrEmpty(value.ToStringEx()))
        return ValidationResult.ValidResult;
      var isDateValidation = IsDate(value);
      if (!isDateValidation.IsValid)
        return new ValidationResult(false, isDateValidation.ErrorContent);

      var time = value.ToString().ToDate();
      return new ValidationResult(time.Date < DateTime.Now.Date, GetDefaultMessage(IsPastDateMessage));
    }

    public static bool IsValid(DependencyObject obj)
    {
      // The dependency object is valid if it has no errors and all
      // of its children (that are dependency objects) are error-free.
      return !Validation.GetHasError(obj) &&
        LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>().All(IsValid);
    }
    public static object GetDefaultMessage(string message)
    {
      return app.TryFindResource(message) ?? messages_enUS[message];
    }
  }

  public class MatchListDP : DependencyObject
  {
    public List<string> MatchList
    {
      get => (List<string>)GetValue(MatchListProperty);
      set => SetValue(MatchListProperty, value);
    }
    public static readonly DependencyProperty MatchListProperty = DependencyProperty.Register(
        nameof(MatchList),
        typeof(List<string>),
        typeof(MatchListDP),
        new PropertyMetadata(new List<string>()));
  }

  public class MatchValidationDP : DependencyObject
  {
    public string Match
    {
      get => (string)GetValue(MatchProperty);
      set => SetValue(MatchProperty, value);
    }
    public static readonly DependencyProperty MatchProperty = DependencyProperty.Register(
        nameof(Match),
        typeof(string),
        typeof(MatchValidationDP),
        new PropertyMetadata(""));
  }
}
