using PocoEmit;

namespace PocoEmitUnitTests.Supports;

public abstract class PocoConvertTestBase
{
    protected readonly Poco _poco = new();
    protected readonly TimeConverter _timeConverter = new();
    public PocoConvertTestBase()
    {
        // 继承Global配置,能被EmitOptions对象引用
        Poco.Global.SetSystemConvert();
        // 多态,覆盖Global配置
        _poco.AddConverter(_timeConverter);
    }

    internal class PocoId(int id)
    {
        private readonly int _id = id;
        public int Id 
            => _id;
    }
}
