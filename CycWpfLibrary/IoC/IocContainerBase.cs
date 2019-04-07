using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycWpfLibrary
{
  public interface IocContainerBase
  {
    bool IsRegistered<TClass>();
    bool IsCreated<TClass>();
    void Register<TClass>();
    void Unregister<TClass>();
    TClass Get<TClass>() where TClass : class;
    void Reset();
  }
}
