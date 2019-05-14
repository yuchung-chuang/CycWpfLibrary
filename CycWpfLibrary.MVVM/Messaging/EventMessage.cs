namespace CycWpfLibrary.MVVM
{
  public class EventMessage : MessageBase<EventMessage>
  {
    public object Args { get; set; }

    public EventMessage()
    {

    }

    public EventMessage(object args) : this()
    {
      Args = args;
    }
  }

  public class EventMessage<TArgs> : MessageBase<EventMessage<TArgs>>
    where TArgs : class
  {
    public EventMessage()
    {

    }

    public EventMessage(TArgs args) : this()
    {
      Args = args;
    }

    public TArgs Args { get; set; }
  }
}
