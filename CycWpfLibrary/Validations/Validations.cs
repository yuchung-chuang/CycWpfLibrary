using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CycWpfLibrary.Validations
{
  public class DoubleValidation : ValidationRule
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.IsDouble(value);
  }

  public class LogBaseValidation : ValidationRule
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationHelpers.IsLogBase(value);
  }
}
