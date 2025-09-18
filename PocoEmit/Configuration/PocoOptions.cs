using System;

namespace PocoEmit.Configuration;

/// <summary>
/// 成员容量配置
/// </summary>
public class PocoOptions
{
    /// <summary>
    /// 并发
    /// </summary>
    public int ConcurrencyLevel = Environment.ProcessorCount;
    /// <summary>
    /// 转化器数量
    /// </summary>
    public int ConverterCapacity = 127;
    /// <summary>
    /// 转化器配置数量
    /// </summary>
    public int ConverterConfigurationCapacity = 31;
    /// <summary>
    /// 成员集合数量
    /// </summary>
    public int MemberBundleCapacity = 127;
    /// <summary>
    /// 是Invoke,还是内嵌表达式
    /// </summary>
    public bool LambdaInvoke = false;
}
