using System;
using System.Reflection;

namespace Injections
{
  public class ConstructorDescription : MethodBaseDescription
  {
    private Func<object[], object> _constructor;

    public ConstructorDescription(ConstructorInfo constructorInfo, Attribute attribute) : base(constructorInfo, attribute, false)
    {
      _constructor = constructorInfo.Invoke;
    }

    public object CreateInstance(Type type, IInjector injector)
    {
      var parameters = GetParameterValues(type, injector);
      try
      {
        return _constructor(parameters);
      }
      catch (Exception exception)
      {
        throw exception;
      }
    }

    public override MemberKind Kind { get { return MemberKind.Constructor; } }
    public override Type Type { get { return null; } }
    public override Type ProviderType { get { return null; } }
  }
}
