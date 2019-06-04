using System;

namespace Injections.Resolvers
{
  public class SingletonResolver : IResolver, IResolverHook
  {
    private Type _value;
    private readonly bool _oneInstance;
    private object _instance;

    public SingletonResolver(Type value, bool oneInstance)
    {
      _value = value;
      _oneInstance = oneInstance;
    }

    public object Resolve(IInjector injector, Type type)
    {
      if (_instance == null)
      {
        var clazz = _value;
        if (_oneInstance && type != clazz)
        {
          _instance = injector.Resolve(clazz);
        }
        if (_instance == null)
        {
          var factory = new FactoryResolver(clazz);
          factory.OnRegister(injector);
          _instance = factory.Create(injector, clazz);
          factory.OnUnRegister();
          injector.Inject(_instance);
        }
      }
      return _instance;
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
