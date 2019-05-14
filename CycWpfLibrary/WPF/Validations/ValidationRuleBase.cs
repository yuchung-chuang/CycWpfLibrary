using System.Windows.Controls;

namespace CycWpfLibrary
{
  public abstract class ValidationRuleBase : ValidationRule
  {
    public string Message { get; set; }
  }
}
