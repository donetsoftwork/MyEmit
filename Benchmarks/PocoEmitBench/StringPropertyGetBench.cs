using BenchmarkDotNet.Attributes;
using FastReflectionLib;
using PocoEmit;
using System.Reflection;

namespace PocoEmitBench;

[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 200000000)]
public class StringPropertyGetBench : PropertyBenchBase
{
    static readonly PropertyInfo _property = typeof(TestClass).GetProperty("StringProperty")!;
    readonly IPropertyAccessor _fastReflect = FastReflectionCaches.PropertyAccessorCache.Get(_property);
    readonly Func<object, object> _hardCode = obj => ((TestClass)obj).StringProperty;
    readonly Func<object, object> _poco = PocoEmit.Poco.Default.GetReadFunc<object, object>(_property);
    readonly Func<object, object> _il = ILPropertyHelper.GetReadFunc<object, object>(_property);
    readonly Func<TestClass, object> _hardCode0 = obj => obj.StringProperty;
    readonly Func<TestClass, object> _poco0 = PocoEmit.Poco.Default.GetReadFunc<TestClass, object>(_property);

    [Benchmark]
    public object? Reflect()
    {
        return _property.GetValue(_obj);
    }

    [Benchmark]
    public object? FastReflect()
    {
        return _fastReflect.GetValue(_obj);
    }
    [Benchmark]
    public object? HardCode()
    {
        return _hardCode(_obj);
    }
    [Benchmark(Baseline = true)]
    public object? Poco()
    {
        return _poco(_obj);
    }
    [Benchmark]
    public object? IL()
    {
        return _il(_obj);
    }
    [Benchmark]
    public object? HardCode0()
    {
        return _hardCode0(_obj);
    }
    [Benchmark]
    public object? Poco0()
    {
        return _poco0(_obj);
    }
}
