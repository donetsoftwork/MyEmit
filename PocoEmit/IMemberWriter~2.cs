namespace PocoEmit;

/// <summary>
/// 成员写入器
/// </summary>
/// <typeparam name="TInstance">实体类型</typeparam>
/// <typeparam name="TValue">成员类型</typeparam>
public interface IMemberWriter<TInstance, TValue>
{
    /// <summary>
    /// 成员名
    /// </summary>
    string Name { get; }
    /// <summary>
    /// 写入
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="value"></param>
    void Write(TInstance instance, TValue value);
}
