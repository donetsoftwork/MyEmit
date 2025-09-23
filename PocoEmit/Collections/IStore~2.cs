namespace PocoEmit.Collections;

/// <summary>
/// 存储(系统行为)
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public interface IStore<TKey, TValue>
{
    /// <summary>
    /// 设置
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    void Set(in TKey key, TValue value);
}
