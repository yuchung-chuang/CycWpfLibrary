using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CycWpfLibrary.CustomControls
{
  [TemplatePart(Name = UpButtonKey, Type = typeof(Button))]
  [TemplatePart(Name = DownButtonKey, Type = typeof(Button))]
  public class NumericTextBox : TextBox
  {
    private const string UpButtonKey = "PART_UpButton";
    private const string DownButtonKey = "PART_DownButton";

    private Button _btnUp = null;
    private Button _btnDown = null;

    static NumericTextBox()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericTextBox),
          new FrameworkPropertyMetadata(typeof(NumericTextBox)));
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();

      _btnUp = Template.FindName(UpButtonKey, this) as Button;
      _btnDown = Template.FindName(DownButtonKey, this) as Button;

      _btnUp.Click += delegate { Operate("+"); };
      _btnDown.Click += delegate { Operate("-"); };
    }

    private void Operate(string operation)
    {
      var input = 0;

      if (int.TryParse(this.Text, out input))
      {
        if (operation == "+")
        {
          this.Text = (input + 1).ToString();
        }
        else
        {
          this.Text = (input - 1).ToString();
        }
      }
    }
  }
}
