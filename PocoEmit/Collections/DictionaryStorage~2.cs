using System.Collections.Concurrent;
using System.Collections.Generic;

namespace PocoEmit.Collections;

/// <summary>
/// 字典存储
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <param name="provider"></param>
public class DictionaryStorage<TKey, TValue>(IDictionary<TKey, TValue> provider)
    : ICacher<TKey, TValue>
    where TKey : notnull
{
    /// <summary>
    /// 字典存储
    /// </summary>
    public DictionaryStorage()
        : this(new ConcurrentDictionary<TKey, TValue>())
    {
    }
    #region 配置
    /// <summary>
    /// 存储字典
    /// </summary>
    protected readonly IDictionary<TKey, TValue> _provider = provider;
    /// <summary>
    /// 存储字典
    /// </summary>
    public IDictionary<TKey, TValue> Provider 
        => _provider;
    #endregion
    #region ISettings<TKey, TValue>
    /// <inheritdoc />
    public bool ContainsKey(in TKey key)
        => _provider.ContainsKey(key);
    /// <inheritdoc />
    public bool TryGetCache(in TKey key, out TValue cached)
        => _provider.TryGetValue(key, out cached);
    /// <inheritdoc />
    public void Set(in TKey key, TValue value)
        => _provider[key] = value;
    #endregion
}
