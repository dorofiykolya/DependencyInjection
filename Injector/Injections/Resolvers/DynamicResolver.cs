using System;

namespace Injections.Resolvers
{
  public class DynamicResolver : IResolver, IResolverHook
  {
    private Func<object> _valueProvider;

    public DynamicResolver(Func<object> valueProvider)
    {
      _valueProvider = valueProvider;
    }

    public DynamicResolver(Func<Type, object> valueProvider)
    {

    }

    public object Resolve(IInjector injector, Type type)
    {
      return _valueProvider();
    }

    public void OnRegister(IInjector injector)
    {

    }

    public void OnUnRegister()
    {
      _valueProvider = null;
    }
  }

  public class DynamicResolver<T> : IResolver, IResolverHook
  {
    private Func<Type, object> _valueProvider;

    public DynamicResolver(Func<Type, object> valueProvider)
    {
      _valueProvider = valueProvider;
    }

    public object Resolve(IInjector injector, Type type)
    {
      return _valueProvider(typeof(T));
    }

    public void OnRegister(IInjector injector)
    {

    }

    public void OnUnRegister()
    {
      _valueProvider = null;
    }
  }
}
