using System;
using System.Reflection;

namespace Injections
{
  public class MethodDescription : MethodBaseDescription
  {
    private readonly MethodInfo _methodInfo;
    private Func<object, object[], object> _invoker;

    public MethodDescription(MethodInfo methodInfo, Attribute attribute) : base(methodInfo, attribute)
    {
      _methodInfo = methodInfo;
      _invoker = methodInfo.Invoke;
    }

    public MethodInfo Info { get { return _methodInfo; } }

    public virtual Type ReturnType { get { return _methodInfo.ReturnType; } }

    public override MemberKind Kind { get { return MemberKind.Method; } }

    public override Type Type { get { return null; } }

    public override Type ProviderType { get { return null; } }

    public override void SetValue(object target, object value)
    {
      throw new NotImplementedException();
    }

    public override object GetValue(object target)
    {
      throw new NotImplementedException();
    }

    public override void Apply(object target, Type targetType, IInjector injector)
    {
      var parameters = GetParameterValues(targetType, injector);
      _invoker(target, parameters);
    }
  }
}
