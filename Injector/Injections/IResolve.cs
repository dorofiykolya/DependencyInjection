using System;

namespace Injections
{
  public interface IResolve
  {
    object Resolve(Type type);
  }
}
