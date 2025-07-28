using PocoEmit;

namespace PocoEmitUnitTests.Supports;

/// <summary>
/// 成员测试基类
/// </summary>
public abstract class PocoMemberTestBase
{
    protected Poco _poco = new();
    protected TestInstance _instance = new();
    public PocoMemberTestBase()
    {
        // 加载System.Convert
        Poco.Global.SetSystemConvert();
    }
}
