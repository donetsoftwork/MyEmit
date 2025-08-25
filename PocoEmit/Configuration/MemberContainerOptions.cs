using System;

namespace PocoEmit.Configuration;

/// <summary>
/// 成员容器配置
/// </summary>
public class MemberContainerOptions
{
    /// <summary>
    /// 并发
    /// </summary>
    public int ConcurrencyLevel = Environment.ProcessorCount;
    /// <summary>
    /// 字段数量
    /// </summary>
    public int FieldCapacity = 997;
    /// <summary>
    /// 属性数量
    /// </summary>
    public int PropertyCapacity = 997;
    /// <summary>
    /// 枚举数量
    /// </summary>
    public int EnumBundleCapacity = 7;
}
