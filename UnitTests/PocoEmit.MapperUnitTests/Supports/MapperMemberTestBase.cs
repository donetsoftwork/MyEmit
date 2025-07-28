namespace PocoEmit.MapperUnitTests.Supports;

/// <summary>
/// 成员测试基类
/// </summary>
public abstract class MapperMemberTestBase
{
    protected Mapper _mapper = new();
    protected TestInstance _instance = new();
    public MapperMemberTestBase()
    {
        // 加载System.Convert
        Mapper.Global.SetSystemConvert();
    }
}
