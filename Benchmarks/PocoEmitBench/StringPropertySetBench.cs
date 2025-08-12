using BenchmarkDotNet.Attributes;
using FastReflectionLib;
using PocoEmit;
using System.Reflection;

namespace PocoEmitBench;

[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 200000000)]
public class StringPropertySetBench : PropertyBenchBase
{
    static readonly PropertyInfo _property = typeof(TestClass).GetProperty("StringProperty")!;
    readonly IPropertyAccessor _fastReflect = FastReflectionCaches.PropertyAccessorCache.Get(_property);
    readonly Action<object, object> _hardCode = (obj, p) => ((TestClass)obj).StringProperty = (string)p;
    readonly Action<object, object> _poco = PocoEmit.Poco.Default.GetWriteAction<object, object>(_property);
    readonly Action<object, object> _il = ILPropertyHelper.GetWriteAction<object, object>(_property);
    readonly Action<TestClass, string> _hardCode0 = (obj, p) => obj.StringProperty = p;
    readonly Action<TestClass, string> _poco0 = PocoEmit.Poco.Default.GetWriteAction<TestClass, string>(_property);
    readonly string _value = "StringPropertySetBench";
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
