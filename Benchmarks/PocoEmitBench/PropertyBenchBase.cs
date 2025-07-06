namespace PocoEmitBench;

public abstract class PropertyBenchBase
{
    internal TestClass _obj = new();

    internal class TestClass
    {
        public object ObjectProperty { get; set; } = new object();
        public string StringProperty { get; set; } = "Hello, World!";
        public int IntProperty { get; set; } = 0;
    }
}
