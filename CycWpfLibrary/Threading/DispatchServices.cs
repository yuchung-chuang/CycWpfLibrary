using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace CycWpfLibrary.Threading
{
  public static class DispatchServices
  {
    private static Dispatcher dispatcher = Application.Current.Dispatcher;
    public static void Invoke(Action action)
    {
      if (dispatcher is null || dispatcher.CheckAccess())
      {
        action();
      }
      else
      {
        try
        {
          dispatcher.Invoke(action);
        }
        catch (TaskCanceledException e)
        {
          Debug.WriteLine(e.Message);
          // expected behavior
        }
      }
    }

    public static TResult Invoke<TResult>(Func<TResult> action)
    {
      if (dispatcher is null || dispatcher.CheckAccess())
      {
        return action();
      }
      else
      {
        try
        {
          return dispatcher.Invoke(action);
        }
        catch (TaskCanceledException e)
        {
          Debug.WriteLine(e.Message);
          // expected behavior
          return default;
        }
      }
    }
  }
}
