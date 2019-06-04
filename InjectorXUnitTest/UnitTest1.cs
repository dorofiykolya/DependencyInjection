using Injections;
using System;
using Xunit;

namespace InjectorXUnitTest
{
  public class UnitTest1
  {
    [Inject]
    private IInjector Injector;

    [Fact]
    public void Test1()
    {
      IInjector injector = new Injector();
      injector.Inject(this);
      injector.ToValue(this);

      Assert.Equal(this, injector.Resolve<UnitTest1>());
      Assert.Equal(injector, injector.Resolve<IInjector>());
    }
  }
}
