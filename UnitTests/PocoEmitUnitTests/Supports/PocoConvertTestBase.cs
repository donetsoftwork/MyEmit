using PocoEmit;

namespace PocoEmitUnitTests.Supports;

public abstract class PocoConvertTestBase
{
    protected readonly IPoco _poco = Poco.Create();
    protected readonly TimeConverter _timeConverter = new();
    public PocoConvertTestBase()
    {
        // 继承Global配置,能被EmitOptions对象引用
        Poco.Global.UseSystemConvert();
        // 多态,覆盖Global配置
        _poco.UseConverter(_timeConverter);
    }

    internal class PocoId(int id)
    {
        public int Id { get; } = id;
    }
}
