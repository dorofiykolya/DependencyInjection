using Injections.Resolvers;
using System;

namespace Injections
{
  public static class InjectorExtensions
  {
    public static void ToValue<T>(this IInjector injector, Func<T> valueProvider)
    {
      injector.Register(typeof(T), new DynamicResolver(() => valueProvider()));
    }

    public static void ToValue<TApi>(this IInjector injector, Func<object> valueProvider)
    {
      injector.Register(typeof(TApi), new DynamicResolver(valueProvider));
    }

    public static void ToValue<TApi, TValue>(this IInjector injector, Func<TValue> valueProvider)
    {
      injector.Register(typeof(TApi), new DynamicResolver<TValue>(valueProvider));
    }

    public static void ToValue(this IInjector injector, object value)
    {
      injector.Register(value.GetType(), new ValueResolver(value));
    }

    public static void ToValue<T>(this IInjector injector, object value)
    {
      injector.Register(typeof(T), new ValueResolver(value));
    }

    public static void ToValue<TApi, TImpl>(this IInjector injector, TImpl value) where TImpl : TApi
    {
      injector.Register(typeof(TApi), new ValueResolver(value));
    }

    public static void ToValue(this IInjector injector, Type api, object value)
    {
      injector.Register(api, new ValueResolver(value));
    }

    public static void ToFactory<T>(this IInjector injector)
    {
      CheckImpl<T>();
      ToFactory(injector, typeof(T));
    }

    public static void ToFactory(this IInjector injector, Type type)
    {
      CheckImpl(type);
      injector.Register(type, new FactoryResolver(type));
    }

    public static void ToFactory<TApi, TImpl>(this IInjector injector) where TImpl : TApi
    {
      CheckImpl<TImpl>();
      injector.Register(typeof(TApi), new FactoryResolver(typeof(TImpl)));
    }

    public static void ToSingleton<T>(this IInjector injector, bool oneInstance = true)
    {
      CheckImpl<T>();
      injector.Register(typeof(T), new SingletonResolver(typeof(T), oneInstance));
    }

    public static void ToSingleton<TApi, TImpl>(this IInjector injector, bool oneInstance = true) where TImpl : TApi
    {
      CheckImpl<TImpl>();
      injector.Register(typeof(TApi), new SingletonResolver(typeof(TImpl), oneInstance));
    }

    public static void ToSingleton(this IInjector injector, Type type, bool oneInstance = true)
    {
      CheckImpl(type);
      injector.Register(type, new SingletonResolver(type, oneInstance));
    }

    public static T Resolve<T>(this IResolve resolver)
    {
      return (T)resolver.Resolve(typeof(T));
    }

    private static void CheckImpl<T>()
    {
      var type = typeof(T);
      if (type.IsInterface || type.IsAbstract)
      {
        throw new ArgumentException();
      }
    }

    private static void CheckImpl(Type type)
    {
      if (type.IsInterface || type.IsAbstract)
      {
        throw new ArgumentException();
      }
    }
  }
}
