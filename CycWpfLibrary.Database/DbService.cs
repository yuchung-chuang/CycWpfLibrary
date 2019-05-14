using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CycWpfLibrary.MVVM;

namespace CycWpfLibrary.Database
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
    /// <summary>
    /// Get list of data with specific type <typeparamref name="TItem"/>
    /// </summary>
    public static async Task<List<TItem>> GetAsync<TItem>(TItem entity)
      where TItem : class
    {
      return await GetAsync<TItem>();
    }
    /// <summary>
    /// Get list of data with specific type <typeparamref name="TItem"/>
    /// </summary>
    public static async Task<List<TItem>> GetAsync<TItem>()
      where TItem : class
    {
      var output = new List<TItem>();
      await UsingDbContextAsync(() =>
      {
        output = dbContext.Get<DbSet<TItem>>().ToList();
      });
      return output;
    }
    /// <summary>
    /// Get list of data satisfying <paramref name="predicate"/> with specific type <typeparamref name="TItem"/> 
    /// </summary>
    public static async Task<List<TItem>> GetAsync<TItem>(Func<TItem, bool> predicate)
      where TItem : class
    {
      var output = new List<TItem>();
      await UsingDbContextAsync(() =>
      {
        output = dbContext.Get<DbSet<TItem>>().Where(predicate).ToList();
      });
      return output;
    }
    /// <summary>
    /// Add <paramref name="item"/> to database
    /// </summary>
    public static async Task<bool> AddAsync<TItem>(TItem item)
      where TItem : class
    {
      return await UsingDbContextAsync(() =>
      {
        dbContext.Get<DbSet<TItem>>().Add(item);
        dbContext.SaveChanges();
      });
    }
    /// <summary>
    /// Remove <paramref name="item"/> in database
    /// </summary>
    public static async Task<bool> RemoveAsync<TItem>(TItem item)
      where TItem : class
    {
      return await UsingDbContextAsync(() =>
      {
        dbContext.Get<DbSet<TItem>>().Remove(item);
        dbContext.SaveChanges();
      });
    }
    /// <summary>
    /// Remove item satisfying <paramref name="predicate"/> in database
    /// </summary>
    public static async Task<bool> RemoveAsync<TItem>(Func<TItem, bool> predicate)
      where TItem : class
    {
      return await SetAsync(predicate, null);
    }
    /// <summary>
    /// Set item satisfying <paramref name="predicate"/> to <paramref name="value"/>
    /// </summary>
    public static async Task<bool> SetAsync<TItem>(Func<TItem, bool> predicate, TItem value)
      where TItem : class
    {
      return await UsingDbContextAsync(() =>
      {
        var item = dbContext.Get<DbSet<TItem>>().FirstOrDefault(predicate);
        dbContext.Get<DbSet<TItem>>().Remove(item);
        dbContext.SaveChanges();
        dbContext.Get<DbSet<TItem>>().Add(value);
        dbContext.SaveChanges();
      });
    }
  }
}
