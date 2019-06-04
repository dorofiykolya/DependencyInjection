using System;
using System.Linq;

namespace Injections.Resolvers
{
  public class FactoryResolver : IResolver, IResolverHook
  {
    private Type _value;

    public FactoryResolver(Type value)
    {
      _value = value;
    }

    public object Resolve(IInjector injector, Type type)
    {
      var targetType = _value;
      var result = CreateInstance(injector, targetType);
      if (result != null)
      {
        injector.Inject(result);
      }
      return result;
    }

    public object Create(IInjector injector, Type type)
    {
      var targetType = _value;
      var result = CreateInstance(injector, targetType);
      return result;
    }

    private object CreateInstance(IInjector injector, Type type)
    {
      var provider = injector.DescriptionProvider.GetProvider(type);
      var constructors = provider.GetByAttribute<InjectAttribute>();
      ConstructorDescription constructor = null;
      if (constructors != null)
      {
        constructor = constructors.FirstOrDefault(m => m.Kind == MemberKind.Constructor) as ConstructorDescription;
      }
      if (constructor == null)
      {
        constructor = provider.DefaultConstructor;
      }
      if (constructor != null)
      {
        return constructor.CreateInstance(type, injector);
      }

      var result = Activator.CreateInstance(type);
      return result;
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
