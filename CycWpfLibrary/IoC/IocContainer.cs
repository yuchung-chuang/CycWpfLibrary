using System;
using System.Collections.Generic;

namespace CycWpfLibrary
{
  public class IocContainer : IocContainerBase
  {
    private readonly Dictionary<Type, Delegate> _factories = new Dictionary<Type, Delegate>();
    private readonly Dictionary<Type, object> _instances = new Dictionary<Type, object>();

    public readonly static IocContainer Default = new IocContainer();

    public bool IsRegistered<TClass>()
    {
      var classType = typeof(TClass);
      return _factories.ContainsKey(classType);
    }

    public bool IsCreated<TClass>()
    {
      var classType = typeof(TClass);
      return _instances.ContainsKey(classType);
    }

    public void Register<TClass>()
    {
      var classType = typeof(TClass);
      if (IsRegistered<TClass>()) // already registered
        return;
      else
      {
        Func<TClass> factory = NativeMethod.MakeInstance<TClass>;
        _factories.Add(classType, factory);
      }
    }

    public void Unregister<TClass>()
    {
      var classType = typeof(TClass);

      _factories.Remove(classType); // return false if not found
      _instances.Remove(classType);
    }

    public TClass Get<TClass>()where TClass : class
    {
      var classType = typeof(TClass);
      if (!IsRegistered<TClass>())
        throw new InvalidOperationException($"{classType} has not registered.");

      if (!IsCreated<TClass>()) // not initiated
      {
        var instance = _factories[classType].DynamicInvoke(null);
        _instances.Add(classType, instance);
        return instance as TClass;
      }
      else // initiated
      {
        return _instances[classType] as TClass;
      }
    }

    public void Reset()
    {
      _factories.Clear();
      _instances.Clear();
    }
  }
}
