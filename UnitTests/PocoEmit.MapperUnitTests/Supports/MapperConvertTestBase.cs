namespace PocoEmit.MapperUnitTests.Supports;

/// <summary>
/// 类型转化基类
/// </summary>
public abstract class MapperConvertTestBase
{
    protected readonly Mapper _mapper = new();
    protected readonly TimeConverter _timeConverter = new();

    public MapperConvertTestBase()
    {
        // 继承Global配置,能被EmitOptions对象引用
        Mapper.Global.SetSystemConvert();
        // 多态,覆盖Global配置
        _mapper.AddConverter(_timeConverter);
    }
    #region Supports
    internal class Manager
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
    }

    internal class ManagerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public UserDTO User { get; set; }
    }

    internal struct StructSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    internal struct StructDest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    internal class MapperId(int id)
    {
        private readonly int _id = id;
        public int Id
            => _id;
    }
    #endregion
}
