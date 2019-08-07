using System;

namespace Injections.Resolvers
{
  public class DynamicResolver : IResolver, IResolverHook
  {
    private Func<object> _valueProvider;
    private Func<Type, object> _valueProviderDynamic;

    public DynamicResolver(Func<object> valueProvider)
    {
      _valueProvider = valueProvider;
    }

    public DynamicResolver(Func<Type, object> valueProvider)
    {
      _valueProviderDynamic = valueProvider;
    }

    public object Resolve(IInjector injector, Type type)
    {
      if (_valueProvider != null)
      {
        return _valueProvider();
      }

      return _valueProviderDynamic(type);
    }

    public void OnRegister(IInjector injector)
    {

    }

    public void OnUnRegister()
    {
      _valueProvider = null;
      _valueProviderDynamic = null;
    }
  }

  public class DynamicResolver<T> : IResolver, IResolverHook
  {
    private Func<T> _valueProvider;

    public DynamicResolver(Func<T> valueProvider)
    {
      _valueProvider = valueProvider;
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
}
