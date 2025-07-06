using System.Collections.Generic;
#if NET9_0_OR_GREATER
using System.Threading;
#endif

namespace PocoEmit.Services;

/// <summary>
/// 缓存基类
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public abstract class CacheBase<TKey, TValue>
    where TKey : notnull
{
    /// <summary>
    /// 缓存字典
    /// </summary>
    private readonly Dictionary<TKey, TValue> _cacher = [];
#if NET9_0_OR_GREATER
    private readonly Lock _cacherLock = new();
#endif
    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public TValue Get(TKey key)
    {
        if (_cacher.TryGetValue(key, out TValue value))
            return value;
#if NET9_0_OR_GREATER
        lock (_cacherLock)
#else
        lock (_cacher)
#endif
        {
            if (_cacher.TryGetValue(key, out value))
                return value;
            value = CreateNew(key);
            _cacher[key] = value;
        }
        return value;
    }
    /// <summary>
    /// 设置值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Set(TKey key, TValue value)
    {
        _cacher[key] = value;
    }
    /// <summary>
    /// 构造新值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected abstract TValue CreateNew(TKey key);
}
