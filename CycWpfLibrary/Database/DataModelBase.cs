using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CycWpfLibrary;

namespace CycChat.Core
{
  public abstract class DataModelBase<TChild> : ObservableObject
    where TChild : DataModelBase<TChild>, new()
  {
    //static DataModelBase()
    //{
    //  InitializeAsync();
    //}
    //private static async void InitializeAsync()
    //{
    //  var props = GetChildType().GetProperties(BindingFlags.Static | BindingFlags.Public);
    //  foreach (var prop in props)
    //  {
    //    var TItem = prop.PropertyType.GetGenericArguments()[0];
    //    var listtype = typeof(List<>);
    //    var generictype = listtype.MakeGenericType(TItem);
    //    var constructor = generictype.GetConstructors(BindingFlags.Public)[0];
    //    dynamic entity = Convert.ChangeType(constructor.Invoke(null), generictype);
    //    GetChildType().Set(await DbServices.GetAsync(entity));
    //  }
    //  //Users = await DbServices.GetAsync<User>();
    //}

    public static Type GetChildType() => new TChild().GetType();

    public static async Task<bool> AddAsync<TItem, TdbContext>(TItem item)
      where TItem : class
      where TdbContext : DbContext, new()
    {
      var flag = await DbService<TdbContext>.AddAsync(item);
      if (flag)
      {
        var childType = GetChildType();
        childType.Get<List<TItem>>().Add(item);

        RaiseStaticPropertyChanged<TItem>(childType);
      }
      return flag;
    }

    public static async Task<bool> SetAsync<TItem, TdbContext>(Func<TItem, bool> predicate, TItem value)
      where TItem : class
      where TdbContext : DbContext, new()
    {
      var flag = await DbService<TdbContext>.SetAsync(predicate, value);
      if (flag)
      {
        var childType = GetChildType();
        var list = childType.Get<List<TItem>>();
        var item = list.First(predicate);
        list.Remove(item);
        list.Add(value);

        RaiseStaticPropertyChanged<TItem>(childType);
      }
      return flag;
    }

    internal static void RaiseStaticPropertyChanged<TItem>(Type childType)
      where TItem : class
    {
      var className = childType.Name;
      var propName = childType.GetPropInfo<List<TItem>>().Name;
      OnStaticPropertyChanged(className, propName);
    }
  }
}
