using System;

namespace Injections
{
  public sealed class Lazy<T>
  {
    public static implicit operator T(Lazy<T> lazy)
    {
      return lazy.Value;
    }

    private readonly Func<object> _factory;
    private T _value;
    private bool _initialized;

    public Lazy(Func<object> factory)
    {
      _factory = factory;
    }

    public T Value
    {
      get
      {
        lock (_factory)
        {
          if (!_initialized)
          {
            _value = (T)_factory();
            _initialized = true;
          }
        }

        return _value;
      }
    }
  }
}
