using System;

namespace Injections
{
  public interface IResolver
  {
    object Resolve(IInjector injector, Type type);
  }

  public interface IResolverHook
  {
    void OnRegister(IInjector injector);
    void OnUnRegister();
  }

}
