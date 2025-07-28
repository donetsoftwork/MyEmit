namespace PocoEmit;

/// <summary>
/// 成员读取器
/// </summary>
/// <typeparam name="TInstance">实体类型</typeparam>
/// <typeparam name="TValue">成员类型</typeparam>
public interface IMemberReader<TInstance, TValue>
{
    /// <summary>
    /// 成员名
    /// </summary>
    string Name { get; }
    /// <summary>
    /// 读取
    /// </summary>
    /// <param name="instance">实体</param>
    /// <returns>成员值</returns>
    TValue Read(TInstance instance);
}