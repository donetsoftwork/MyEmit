namespace PocoEmit.Collections;

/// <summary>
/// 数据缓存
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public interface ICacher<TKey, TValue>
    : IStore<TKey, TValue>
{
    /// <summary>
    /// 判断是否存在
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    bool ContainsKey(TKey key);
    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    bool TryGetValue(TKey key, out TValue value);
}

