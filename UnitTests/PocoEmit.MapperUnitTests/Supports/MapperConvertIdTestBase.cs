namespace PocoEmit.MapperUnitTests.Supports;

public abstract class MapperConvertIdTestBase
{
    protected readonly IMapper _mapper = Mapper.Create();

    public MapperConvertIdTestBase()
    {
        // 继承Global配置,能被EmitOptions对象引用
        Mapper.Global.UseSystemConvert();
    }
    #region Supports
    internal class MyMapperId(int id)
    {
        public int Id { get; } = id;
    }
    internal class MyMapperId2(int? id)
    {
        public int? Id { get; } = id;
    }
    internal class MyMapper(MyMapperId id)
    {
        public MyMapperId Id { get; } = id;
        public string Name { get; set; }
    }
    internal class MyMapperDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    internal class MyMapperDTO2
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
    }
    #endregion
}
