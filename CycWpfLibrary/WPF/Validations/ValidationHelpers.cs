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
    private static readonly string NotNullMessageKey = "ValidationMessageNotNull";
    private static readonly string DoubleMessageKey = "ValidationMessageDouble";
    private static readonly string LogBaseMessageKey = "ValidationMessageLogBase";
    private static readonly string ByteMessageKey = "ValidationMessageByte";
    private static readonly string RangeMessageKey = "ValidationMessageRange";
    private static readonly string DateMessageKey = "ValidationMessageDate";
    private static readonly string FutureDateMessageKey = "ValidationMessageFutureDate";
    private static readonly string PastDateMessageKey = "ValidationMessagePastDate";
    #endregion

    public static ValidationResult IsNotNull(object value, string message = null)
    {
      return new ValidationResult(!string.IsNullOrWhiteSpace(value.ToStringEx()), message ?? app.TryFindResource(NotNullMessageKey) ?? messages_enUS[NotNullMessageKey]);
    }

    public static ValidationResult IsDouble(object value)
    {
      if (string.IsNullOrEmpty(value.ToStringEx()))
        return ValidationResult.ValidResult; // allows value to be null
      return new ValidationResult(double.TryParse(value.ToString(), out var num), app.TryFindResource(DoubleMessageKey) ?? messages_enUS[DoubleMessageKey]);
    }

    public static ValidationResult IsLogBase(object value)
    {
      if (string.IsNullOrEmpty(value.ToStringEx()))
        return ValidationResult.ValidResult;
      var doubleValidation = IsDouble(value);
      if (!doubleValidation.IsValid)
        return new ValidationResult(false, doubleValidation.ErrorContent);

      var logBase = value.ToString().ToDouble();
      return new ValidationResult(logBase > 0, app.TryFindResource(LogBaseMessageKey) ?? messages_enUS[LogBaseMessageKey]);
    }

    public static ValidationResult IsByte(object value)
    {
      if (string.IsNullOrEmpty(value.ToStringEx()))
        return ValidationResult.ValidResult;
      var doubleValidation = IsDouble(value);
      if (!doubleValidation.IsValid)
        return new ValidationResult(false, doubleValidation.ErrorContent);

      var @byte = value.ToString().ToDouble();
      return new ValidationResult(Math.IsIn(@byte, 255, 0), app.TryFindResource(ByteMessageKey) ?? messages_enUS[ByteMessageKey]);
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
        app.TryFindResource(RangeMessageKey) ?? messages_enUS[RangeMessageKey] +
        $"{(excludeMin ? "[" : "(")}" +
        $"{(min == int.MinValue ? "-∞" : min.ToString())}, " +
        $"{(max == int.MaxValue ? "∞" : max.ToString())}" +
        $"{(excludeMax ? "]" : ")")}");
    }

    public static ValidationResult IsDate(object value)
    {
      if (string.IsNullOrEmpty(value.ToStringEx()))
        return ValidationResult.ValidResult;
      return new ValidationResult(DateTime.TryParse(value.ToStringEx(), CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces, out var time), app.TryFindResource(DateMessageKey) ?? messages_enUS[DateMessageKey]);
    }

    public static ValidationResult IsFutureDate(object value)
    {
      if (string.IsNullOrEmpty(value.ToStringEx()))
        return ValidationResult.ValidResult;
      var isDateValidation = IsDate(value);
      if (!isDateValidation.IsValid)
        return new ValidationResult(false, isDateValidation.ErrorContent);

      var time = value.ToString().ToDate();
      return new ValidationResult(time.Date > DateTime.Now.Date, app.TryFindResource(FutureDateMessageKey) ?? messages_enUS[FutureDateMessageKey]);
    }

    public static ValidationResult IsPastDate(object value)
    {
      if (string.IsNullOrEmpty(value.ToStringEx()))
        return ValidationResult.ValidResult;
      var isDateValidation = IsDate(value);
      if (!isDateValidation.IsValid)
        return new ValidationResult(false, isDateValidation.ErrorContent);

      var time = value.ToString().ToDate();
      return new ValidationResult(time.Date < DateTime.Now.Date, app.TryFindResource(PastDateMessageKey) ?? messages_enUS[PastDateMessageKey]);
    }

    public static bool IsValid(DependencyObject obj)
    {
      // The dependency object is valid if it has no errors and all
      // of its children (that are dependency objects) are error-free.
      return !Validation.GetHasError(obj) &&
        LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>().All(IsValid);
    }
  }

  public class StringListMarkup : MarkupExtension
  {
    public class StringListDP : DependencyObject
    {
      public List<string> StringCollection
      {
        get => (List<string>)GetValue(StringCollectionProperty);
        set => SetValue(StringCollectionProperty, value);
      }
      public static readonly DependencyProperty StringCollectionProperty = DependencyProperty.Register(
          nameof(StringCollection),
          typeof(List<string>),
          typeof(StringListDP),
          new PropertyMetadata(new List<string>()));
    }

    private StringListDP DP { get; set; } = new StringListDP();
    public PropertyPath Path { get; set; }
    public object Source { get; set; }
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      var binding = new Binding
      {
        Path = Path,
        Source = Source,
      };
      BindingOperations.SetBinding(DP, StringListDP.StringCollectionProperty, binding);
      return DP.StringCollection;
    }
  }
}
