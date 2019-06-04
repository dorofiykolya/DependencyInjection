using System;
using System.Reflection;

namespace Injections
{
  public class PropertyDescription : MemberDescription
  {
    private readonly Type _type;
    private Func<object, object> _getValue;
    private Action<object, object> _setValue;
    private Type _propertyType;

    public PropertyDescription(PropertyInfo propertyInfo, Attribute attribute) : base(propertyInfo, attribute)
    {
      if (propertyInfo != null)
      {
        _getValue = propertyInfo.GetValue;
        _setValue = propertyInfo.SetValue;
        _propertyType = propertyInfo.PropertyType;

        _type = propertyInfo.PropertyType;
        if (_type.IsGenericType && _type.GetGenericTypeDefinition() == typeof(Lazy<>))
        {
          _type = _type.GetGenericArguments()[0];
        }
      }
    }

    public override MemberKind Kind
    {
      get { return MemberKind.Property; }
    }

    public override Type Type
    {
      get { return _propertyType; }
    }

    public override Type ProviderType
    {
      get { return _type; }
    }

    public override void SetValue(object target, object value)
    {
      if (_setValue != null)
      {
        _setValue(target, value);
      }
    }

    public override object GetValue(object target)
    {
      if (_getValue != null)
      {
        return _getValue(target);
      }
      return null;
    }

    public override void Apply(object target, Type targetType, IInjector injector)
    {
      var provider = injector.GetResolver(ProviderType, true);
      SetValue(target, provider.Resolve(injector, targetType));
    }
  }
}
