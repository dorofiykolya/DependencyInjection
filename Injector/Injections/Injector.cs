using System;
using System.Collections.Generic;

namespace Injections
{
  public class Injector : IInjector
  {
    private readonly Dictionary<Type, IResolver> _resolvers;
    public DescriptionProvider DescriptionProvider { get; }
    public IInjector Parent { get; }

    public Injector() : this(null) { }

    public Injector(IInjector parent)
    {
      if (parent != null)
      {
        Parent = parent;
        DescriptionProvider = parent.DescriptionProvider;
      }
      else
      {
        DescriptionProvider = new DescriptionProvider();
        DescriptionProvider.MapAttribute<InjectAttribute>();
      }

      _resolvers = new Dictionary<Type, IResolver>();

      this.ToValue<IInjector>(this);
      this.ToValue<IInject>(this);
      this.ToValue(this);
    }

    public void Register(Type type, IResolver resolver)
    {
      _resolvers[type] = resolver ?? throw new NullReferenceException();
      var hook = resolver as IResolverHook;
      hook?.OnRegister(this);
    }

    public void UnRegister(Type type)
    {
      IResolver resolver;
      if (_resolvers.TryGetValue(type, out resolver))
      {
        var hook = resolver as IResolverHook;
        hook?.OnUnRegister();
        _resolvers.Remove(type);
      }
    }

    public IResolver GetResolver(Type type, bool includeInParents)
    {
      IResolver resolver;
      if (!_resolvers.TryGetValue(type, out resolver) && includeInParents && Parent != null)
      {
        resolver = Parent.GetResolver(type, true);
      }
      return resolver;
    }

    public void Inject(object value)
    {
      if (value != null)
      {
        var typeProvider = DescriptionProvider.GetProvider(value.GetType());
        ApplyResolver(value, typeProvider);
      }
    }

    public object Resolve(Type type)
    {
      var resolver = GetResolver(type, true);
      if (resolver != null)
      {
        return resolver.Resolve(this, type);
      }
      return null;
    }

    private void ApplyResolver(object value, TypeProvider typeProvider)
    {
      if (typeProvider.Parent != null)
      {
        ApplyResolver(value, typeProvider.Parent);
      }

      var members = typeProvider.GetByAttribute<InjectAttribute>(true);
      if (members != null)
      {
        foreach (var member in members)
        {
          var kind = member.Kind;
          if ((kind & MemberKind.Field) == MemberKind.Field ||
              (kind & MemberKind.Property) == MemberKind.Property)
          {
            var provider = GetResolver(member.ProviderType, true);
            if (provider != null)
            {
              if (member.ProviderType != member.Type)
              {
                member.SetValue(value, CreateLazy(provider, member.Type, member.ProviderType));
              }
              else
              {
                member.SetValue(value, provider.Resolve(this, member.Type));
              }
            }
          }
        }
      }
    }

    private object CreateLazy(IResolver provider, Type type, Type providerType)
    {
      Func<object> factory = () => provider.Resolve(this, type);
      return Activator.CreateInstance(typeof(Lazy<>).MakeGenericType(providerType), factory);
    }
  }
}
