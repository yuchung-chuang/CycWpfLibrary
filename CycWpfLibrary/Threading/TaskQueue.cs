using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycWpfLibrary.Threading
{
  /// <summary>
  /// 背景工作隊列物件，可動態加入背景工作並使工作依序執行
  /// </summary>
  public class TaskQueue
  {
    private Queue<Task> queue;
    public int MaxTasks = 10;
    public TaskQueue()
    {
      queue = new Queue<Task>();
    }
    public TaskQueue(int maxTasks) : this()
    {
      MaxTasks = maxTasks;
    }

    /// <summary>
    /// 加入新的工作到隊列中
    /// </summary>
    /// <param name="task">新工作</param>
    public void Enqueue(Task task)
    {
      // 若隊列中工作太多，不能加入新工作
      if (queue.Count > MaxTasks)
      {
        return;
      }
      // 工作完成後移出隊列
      task.ContinueWith(_ => { queue.Dequeue(); });
      // 鏈接前一個工作
      if (queue.Count > 0)
      {
        queue.Last().ContinueWith(_ => { task.Start(); }); 
      }
      // 加入隊列
      queue.Enqueue(task);
      // 開始執行第一個工作
      if (queue.Count == 1)
      {
        task.Start(); 
      }
    }

    /// <summary>
    /// 加入新的工作到隊列中
    /// </summary>
    /// <param name="action">要執行的動作</param>
    public void Enqueue(Action action)
    {
      var task = new Task(action);
      Enqueue(task);
    }

  }
}
