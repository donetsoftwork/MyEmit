#if NET9_0_OR_GREATER
using System.Threading;
#endif

namespace PocoEmit.Collections;

/// <summary>
/// 缓存基类
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public abstract class CacheBase<TKey, TValue>(ICacher<TKey, TValue> cacher)
    : ICacher<TKey, TValue>
{
    /// <summary>
    /// 缓存基类
    /// </summary>
    public CacheBase()
        : this(new DictionaryStorage<TKey, TValue>())
    { 
    }
    /// <summary>
    /// 缓存字典
    /// </summary>
    private readonly ICacher<TKey, TValue> _cacher = cacher;
#if NET9_0_OR_GREATER
    private readonly Lock _cacherLock = new();
#endif
    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual TValue Get(TKey key)
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
            _cacher.Set(key, value);
        }
        return value;
    }
    /// <inheritdoc />
    public bool ContainsKey(TKey key)
        => _cacher.ContainsKey(key);
    /// <inheritdoc />
    public bool TryGetValue(TKey key, out TValue value)
        => _cacher.TryGetValue(key, out value);
    /// <inheritdoc />
    public void Set(TKey key, TValue value)
        => _cacher.Set(key, value);
    /// <summary>
    /// 构造新值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected abstract TValue CreateNew(TKey key);
}
