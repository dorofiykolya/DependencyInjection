using System;
using System.Collections.Generic;
using System.Reflection;

namespace Injections.Resolvers
{
  public class MethodResolver : IResolver, IResolverHook
  {
    private MethodInfo _methodInfo;
    private object _value;
    private readonly bool _optional;

    public MethodResolver(MethodInfo methodInfo, object value, bool optional = false)
    {
      _methodInfo = methodInfo;
      _value = value;
      _optional = optional;
    }

    public object Resolve(IInjector injector, Type type)
    {
      var parameters = GatherParameterValues(type, injector);
      return _methodInfo.Invoke(_value, parameters);
    }

    protected virtual object[] GatherParameterValues(Type targetType, IInjector injector)
    {
      if (_methodInfo == null)
        return new object[0];
      List<object> parameters = new List<object>();
      ParameterInfo[] parameterInfos = _methodInfo.GetParameters();
      int length = parameterInfos.Length;
      for (int i = 0; i < length; i++)
      {
        Type parameterType = parameterInfos[i].ParameterType;
        var isLazy = parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(Lazy<>);
        var provider = injector.GetResolver(isLazy ? parameterType.GetGenericArguments()[0] : parameterType, true);
        if (provider == null)
        {
          if (parameterInfos[i].IsOptional)
          {
            parameters.Add(parameterInfos[i].DefaultValue);
            continue;
          }
          if (_optional)
          {
            parameters.Add(null);
            continue; //TODO: Check optional parameters are in order (last) for this break to work, else use continue
          }
          throw new InvalidOperationException(
            "Injector is missing a mapping to handle constructor injection into target type '"
            + targetType.FullName + "'. \nTarget dependency: " + parameterType.FullName +
            ", method: " + _methodInfo.Name + ", parameter: " + (i + 1)
          );
        }
        parameters.Add(provider.Resolve(injector, targetType));
      }
      return parameters.ToArray();
    }

    public void OnRegister(IInjector injector)
    {

    }

    public void OnUnRegister()
    {
      _methodInfo = null;
      _value = null;
    }
  }
}
