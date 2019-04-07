using CycWpfLibrary;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CycWpfLibrary
{
  /// <summary>
  /// Provide generic database service.
  /// </summary>
  /// <typeparam name="TdbContext">Type of <see cref="DbContext"/></typeparam>
  public static class DbService<TdbContext> where TdbContext : DbContext, new()
  {
    private static readonly int msTimeout = 2000;
    private static TdbContext dbContext;

    /// <summary>
    /// Test that the server is connected
    /// </summary>
    /// <returns>true if the connection is opened</returns>
    private static async Task<bool> CheckConnectionAsync()
    {
      var flag = false;
      string sqlMessage = null;
      try
      {
        var cts = new CancellationTokenSource(msTimeout);
        await NativeMethod.CursorWaitForAsync(() =>
        {
          using (var dbContext = new TdbContext())
          {
            try
            {
              dbContext.Database.Connection.Open();
              flag = true;
            }
            catch (SqlException e)// force timeout, since there is no way to catch exception inside Task.Run
            {
              sqlMessage = e.Message;
              Task.Delay(msTimeout).Wait();
            }
          }
        }, cts.Token);
      }
      catch (TaskCanceledException)
      {
        Messenger.Default.Send(new Message<string>(sqlMessage ?? "Database connection timeout!"), errorToken);
      }
      return flag;
    }
    /// <summary>
    /// Manipulate <see cref="dbContext"/> with <paramref name="action"/> 
    /// after <see cref="CheckConnectionAsync"/>.
    /// </summary>
    /// <returns>True if successful.</returns>
    private static async Task<bool> UsingDbContextAsync(Action action)
    {
      var flag = await CheckConnectionAsync();
      if (flag)
      {
        dbContext = new TdbContext();

        action.Invoke();

        dbContext.Dispose();
      }
      return flag;
    }

    public static readonly object errorToken = new object();
    public static async Task<List<T>> GetAsync<T>(T entity)
      where T : class
    {
      return await GetAsync<T>();
    }
    public static async Task<List<T>> GetAsync<T>() where T : class
    {
      var output = new List<T>();
      await UsingDbContextAsync(() =>
      {
        output = dbContext.Get<DbSet<T>>().ToList();
      });
      return output;
    }
    public static async Task<bool> AddAsync<T>(T item) where T : class
    {
      return await UsingDbContextAsync(() =>
      {
        dbContext.Get<DbSet<T>>().Add(item);
        dbContext.SaveChanges();
      });
    }
  }
}
