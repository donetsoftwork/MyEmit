namespace PocoEmit.MapperUnitTests.Supports;

/// <summary>
/// 成员测试基类
/// </summary>
public abstract class MapperMemberTestBase
{
    protected IMapper _mapper = Mapper.Create();
    protected TestInstance _instance = new();
    public MapperMemberTestBase()
    {
        // 加载System.Convert
        //Mapper.Default.UseSystemConvert();
    }
}
