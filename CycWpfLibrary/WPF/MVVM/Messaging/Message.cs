
namespace CycWpfLibrary
{
  /// <summary>
  /// Passes a null Message to a recipient as an event
  /// </summary>
  ////[ClassInfo(typeof(Messenger))]
  public class Message : MessageBase
  {
    public readonly static Message Empty = new Message();
  }
}