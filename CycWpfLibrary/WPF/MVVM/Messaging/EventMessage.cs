using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycWpfLibrary
{
  public class EventMessage<TArgs> : MessageBase
    where TArgs : class
  {
    public EventMessage(TArgs args)
    {
      Args = args;
    }

    public TArgs Args { get; set; }
  }
}
