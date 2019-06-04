using System;

namespace Injections
{
  public interface IInjector : IInject, IResolve
  {
    void Register(Type type, IResolver resolver);
    void UnRegister(Type type);
    IResolver GetResolver(Type type, bool includeInParents);
    DescriptionProvider DescriptionProvider { get; }
  }
}
