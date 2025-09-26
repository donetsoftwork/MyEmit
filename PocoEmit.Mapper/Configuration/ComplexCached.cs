namespace PocoEmit.Configuration;

/// <summary>
/// 被缓存状态
/// </summary>
public enum ComplexCached
{
    /// <summary>
    /// 从不缓存
    /// </summary>
    Never = 0,
    /// <summary>
    /// 缓存循环引用
    /// </summary>
    Circle = 1,
    /// <summary>
    /// 总是缓存
    /// </summary>
    Always = 2
}
