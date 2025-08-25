namespace PocoEmit.Configuration;

/// <summary>
/// 成员容量配置
/// </summary>
public class MapperOptions : PocoOptions
{
    /// <summary>
    /// 转化器数量
    /// </summary>
    public int CopierCapacity = 31;
    /// <summary>
    /// 转化器配置数量
    /// </summary>
    public int CopierConfigurationCapacity = 31;
    /// <summary>
    /// 激活器配置数量
    /// </summary>
    public int ActivatorConfigurationCapacity = 31;
    /// <summary>
    /// 带参激活器配置数量
    /// </summary>
    public int ArgumentActivatorConfigurationCapacity = 31;
    /// <summary>
    /// 转化后检查成员配置数量
    /// </summary>
    public int CheckMembersCapacity = 31;
    /// <summary>
    /// 成员匹配配置数量
    /// </summary>
    public int MatchCapacity = 31;
    /// <summary>
    /// 基础类型配置数量
    /// </summary>
    public int PrimitiveCapacity = 31;
    /// <summary>
    /// 默认值配置数量
    /// </summary>
    public int DefaultValueCapacity = 31;
}
