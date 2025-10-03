namespace PocoEmit.Collections;

/// <summary>
/// 数据缓存(系统行为)
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
    bool ContainsKey(in TKey key);
    /// <summary>
    /// 尝试获取
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cached"></param>
    /// <returns></returns>
    bool TryGetCache(in TKey key, out TValue cached);
}

