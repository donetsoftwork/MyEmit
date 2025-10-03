using PocoEmit.Collections;
using PocoEmit.Configuration;
using System;

namespace PocoEmit;

/// <summary>
/// Emit扩展方法
/// </summary>
public static partial class PocoEmitServices
{
    #region ISettings<TKey, TValue>
    /// <summary>
    /// 获取
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="settings"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    internal static TValue GetCache<TKey, TValue>(this ICacher<TKey, TValue> settings, TKey key)
    {
        settings.TryGetCache(key, out var value);
        return value;
    }
    /// <summary>
    /// 获取
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="settings"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    internal static TValue Get<TValue>(this CacheBase<PairTypeKey, TValue> settings, Type left, Type right)
        => settings.Get(new PairTypeKey(left, right));
    /// <summary>
    /// 尝试设置缓存不覆盖
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cacher"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    internal static TValue TryCache<TKey, TValue>(this ICacher<TKey, TValue> cacher, TKey key, TValue value)
    {
        // 如果值不为null，则不覆盖
        if (cacher.ContainsKey(key) && cacher.GetCache(key) is TValue value0)
            return value0;
        cacher.Set(key, value);
        return value;
    }
    #endregion
}
