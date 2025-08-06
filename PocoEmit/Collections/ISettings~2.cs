namespace PocoEmit.Collections;

/// <summary>
/// 配置设置
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public interface ISettings<TKey, TValue>
{
    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    bool TryGetValue(TKey key, out TValue value);
    /// <summary>
    /// 设置
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    void Set(TKey key, TValue value);
}
