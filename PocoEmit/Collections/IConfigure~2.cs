namespace PocoEmit.Collections;

/// <summary>
/// 配置(用户行为)
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public interface IConfigure<TKey, TValue>
{
    /// <summary>
    /// 配置
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    void Configure(in TKey key, TValue value);
}
