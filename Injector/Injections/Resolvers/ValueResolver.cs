using System;

namespace Injections.Resolvers
{
  public class ValueResolver : IResolver, IResolverHook
  {
    private object _value;

    public ValueResolver(object value)
    {
      _value = value;
    }

    public object Resolve(IInjector injector, Type type)
    {
      return _value;
    }

    public void OnRegister(IInjector injector)
    {

    }

    public void OnUnRegister()
    {
      _value = null;
    }
  }
}
