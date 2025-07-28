using PocoEmit.Collections;

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
    public static TValue Get<TKey, TValue>(this ISettings<TKey, TValue> settings, TKey key)
    {
        settings.TryGetValue(key, out var value);
        return value;
    }
    /// <summary>
    /// 尝试设置不覆盖
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="settings"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TValue TrySet<TKey, TValue>(this ISettings<TKey, TValue> settings, TKey key, TValue value)
    {
        // 如果值不为null，则不覆盖
        if (settings.ContainsKey(key) && settings.Get(key) is TValue value0)
            return value0;
        settings.Set(key, value);
        return value;
    }
    #endregion
}
