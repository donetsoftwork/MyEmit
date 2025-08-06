namespace PocoEmit.Collections;

/// <summary>
/// 配置读写
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public interface IConfiguration<TKey, TValue>
    : IConfigure<TKey, TValue>
{
    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    bool TryRead(TKey key, out TValue value);
}
