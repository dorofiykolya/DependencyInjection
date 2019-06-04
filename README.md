# Dependency Injection (simple and fast)

### Example1
```csharp
var value = new object();
var injector = new Injector();
injector.ToValue(value);
Assert.True( value == injector.Resolve<object>() );
```

### Example2
```csharp
var random1 = new Random(0);
var injector = new Injector();
injector.ToValue<int>(() => random1.Next());

var random1Value1 = injector.Resolve<int>();
var random1Value2 = injector.Resolve<int>();
var random1Value3 = injector.Resolve<int>();

var random2 = new Random(0);
var random2Value1 = random2.Next();
var random2Value2 = random2.Next();
var random2Value3 = random2.Next();

Assert.Equal(random1Value1, random2Value1);
Assert.Equal(random1Value2, random2Value2);
Assert.Equal(random1Value3, random2Value3);
```

### Example3
```csharp
public interface ITestValue {  }

public class TestValue : ITestValue
{
  [Inject]
  public int Value;
}

public interface ITestInterface {  }

public class TestImplementation : ITestInterface
{
  public ITestValue Value;
}

public class TestClass
{
  [Inject]
  private ITestInterface _testPivateField;

  [Inject]
  public ITestInterface TestProperty { get; set; }

  [Inject]
  public TestValue TestValue;

  public ITestInterface TestPivateField => _testPivateField;
}

var injector1 = new Injector();
injector1.ToValue<int>(10);
injector1.ToFactory<ITestValue, TestValue>();
injector1.ToSingleton<TestValue>();

Assert.Equal(10, injector1.Resolve<int>());
Assert.NotEqual(injector1.Resolve<ITestValue>(), injector1.Resolve<TestValue>());
Assert.Equal(injector1.Resolve<TestValue>(), injector1.Resolve<TestValue>());

var injector2 = new Injector(injector1);
injector2.ToFactory<TestClass>();
injector2.ToFactory<ITestInterface, TestImplementation>();

var testClass1 = new TestClass();
injector2.Inject(testClass1);

Assert.Equal(10, injector2.Resolve<int>());

Assert.NotNull(testClass1.TestValue);
Assert.NotNull(testClass1.TestPivateField);
Assert.NotNull(testClass1.TestProperty);
Assert.NotEqual(testClass1.TestPivateField, testClass1.TestProperty);

var testClass2 = injector2.Resolve<TestClass>();

Assert.NotNull(testClass2.TestValue);
Assert.NotNull(testClass2.TestPivateField);
Assert.NotNull(testClass2.TestProperty);
Assert.NotEqual(testClass2.TestPivateField, testClass2.TestProperty);

Assert.Equal(testClass1.TestValue, testClass2.TestValue);

Assert.Equal(10, testClass1.TestValue.Value);
Assert.Equal(10, testClass2.TestValue.Value);
```
