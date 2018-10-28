using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycWpfLibrary.Controls
{
  public class AxisDesignModel : Axis
  {
    public static AxisDesignModel instance { get; private set; } = new AxisDesignModel();

    public AxisDesignModel()
    {
      AxisLeft = 100;
      AxisTop = 100;
      AxisWidth = 150;
      AxisHeight = 100;
      AxisBrush = "Red";
      ShadowBrush = "#99999999";
    }
  }
}
