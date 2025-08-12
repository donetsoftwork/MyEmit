using PocoEmit;

namespace PocoEmitUnitTests.Supports;

/// <summary>
/// 成员测试基类
/// </summary>
public abstract class PocoMemberTestBase
{
    protected IPoco _poco = Poco.Create();
    protected TestInstance _instance = new();
    public PocoMemberTestBase()
    {
        // 加载System.Convert
        Poco.Default.UseSystemConvert();
    }
}
