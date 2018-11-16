using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CycWpfLibrary.Validations
{
  public static class ValidationHelpers
  {
    public static ValidationResult IsDouble(object value)
    {
      try
      {
        double.Parse(value.ToString());
        return ValidationResult.ValidResult;
      }
      catch (FormatException)
      {
        return new ValidationResult(false, "Input number is invalid");
      }
    }

    public static ValidationResult IsLogBase(object value)
    {
      var doubleValidation = IsDouble(value);
      if (!doubleValidation.IsValid)
      {
        return new ValidationResult(false, doubleValidation.ErrorContent);
      }

      var logBase = double.Parse(value.ToString());
      if (logBase > 0)
      {
        return ValidationResult.ValidResult;
      }
      else
      {
        return new ValidationResult(false, "Log Base must larger than 0");
      }
    }
  }
}
