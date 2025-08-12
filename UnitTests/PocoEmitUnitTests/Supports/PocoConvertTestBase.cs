using PocoEmit;

namespace PocoEmitUnitTests.Supports;

public abstract class PocoConvertTestBase
{
    protected readonly IPoco _poco;
    protected readonly TimeConverter _timeConverter = new();
    public PocoConvertTestBase()
    {
        // Global配置
        Poco.GlobalConfigure(poco => {
            poco.UseSystemConvert()
                .UseConverter(_timeConverter);
        });
        _poco = Poco.Create();
    }

    internal class PocoId(int id)
    {
        public int Id { get; } = id;
    }
}
