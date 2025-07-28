using BenchmarkDotNet.Attributes;
using FastReflectionLib;
using PocoEmit;
using System.Reflection;

namespace PocoEmitBench;

[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 200000000)]
public class ObjectPropertySetBench : PropertyBenchBase
{
    static readonly PropertyInfo _property = typeof(TestClass).GetProperty("ObjectProperty")!;
    readonly IPropertyAccessor _fastReflect = FastReflectionCaches.PropertyAccessorCache.Get(_property);
    readonly Action<object, object> _hardCode = (obj, p) => ((TestClass)obj).ObjectProperty = p;
    readonly Action<object, object> _poco = PocoEmit.Poco.Global.GetWriteAction<object, object>(_property);
    readonly Action<object, object> _il = ILPropertyHelper.GetWriteAction<object, object>(_property);
    readonly Action<TestClass, object> _hardCode0 = (obj, p) => obj.ObjectProperty = p;
    readonly Action<TestClass, object> _poco0 = PocoEmit.Poco.Global.GetWriteAction<TestClass, object>(_property);

    [Benchmark]
    public void Reflect()
    {
        _property.SetValue(_obj, _obj);
    }

    [Benchmark]
    public void FastReflect()
    {
        _fastReflect.SetValue(_obj, _obj);
    }
    [Benchmark]
    public void HardCode()
    {
        _hardCode(_obj, _obj);
    }
    [Benchmark(Baseline = true)]
    public void Poco()
    {
        _poco(_obj, _obj);
    }
    [Benchmark]
    public void IL()
    {
        _il(_obj, _obj);
    }
    [Benchmark]
    public void HardCode0()
    {
        _hardCode0(_obj, _obj);
    }
    [Benchmark]
    public void Poco0()
    {
        _poco0(_obj, _obj);
    }
}
