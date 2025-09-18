namespace PocoEmit.Builders;

/// <summary>
/// 包装器
/// </summary>
/// <typeparam name="TOriginal">原始类型</typeparam>
public interface IWrapper<out TOriginal>
{
    /// <summary>
    /// 原始对象
    /// </summary>
    TOriginal Original { get; }
}
