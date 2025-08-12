using BenchmarkDotNet.Attributes;
using FastReflectionLib;
using PocoEmit;
using System.Reflection;

namespace PocoEmitBench;

[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 200000000)]
public class IntPropertySetBench : PropertyBenchBase
{
    static readonly PropertyInfo _property = typeof(TestClass).GetProperty("IntProperty")!;
    readonly IPropertyAccessor _fastReflect = FastReflectionCaches.PropertyAccessorCache.Get(_property);
    readonly Action<object, object> _hardCode = (obj, p) => ((TestClass)obj).IntProperty = (int)p;
    readonly Action<object, object> _poco = PocoEmit.Poco.Default.GetWriteAction<object, object>(_property);
    readonly Action<object, object> _il = ILPropertyHelper.GetWriteAction<object, object>(_property);
    readonly Action<TestClass, int> _hardCode0 = (obj, p) => obj.IntProperty = p;
    readonly Action<TestClass, int> _poco0 = PocoEmit.Poco.Default.GetWriteAction<TestClass, int>(_property);
    readonly int _value = 42;
    [Benchmark]
    public void Reflect()
    {
        _property.SetValue(_obj, _value);
    }

    [Benchmark]
    public void FastReflect()
    {
        _fastReflect.SetValue(_obj, _value);
    }
    [Benchmark]
    public void HardCode()
    {
        _hardCode(_obj, _value);
    }
    [Benchmark(Baseline = true)]
    public void Poco()
    {
        _poco(_obj, _value);
    }
    [Benchmark]
    public void IL()
    {
        _il(_obj, _value);
    }
    [Benchmark]
    public void HardCode0()
    {
        _hardCode0(_obj, _value);
    }
    [Benchmark]
    public void Poco0()
    {
        _poco0(_obj, _value);
    }
}
