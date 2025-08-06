using PocoEmit.Converters;
using System.Collections.Concurrent;
using PocoEmit.Collections;
using System;
#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
using System.Collections.Generic;
using System.Collections.Frozen;
#endif

namespace PocoEmit.Configuration;

/// <summary>
/// Emit配置
/// </summary>
public abstract partial class ConfigurationBase
    : IPocoOptions
{
    #region 缓存数据
#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    /// <summary>
    /// 转换器缓存
    /// </summary>
    private IDictionary<MapTypeKey, IEmitConverter> _converters = new ConcurrentDictionary<MapTypeKey, IEmitConverter>();
    /// <summary>
    /// 转换器配置
    /// </summary>
    private IDictionary<MapTypeKey, IEmitConverter> _convertConfiguration = new ConcurrentDictionary<MapTypeKey, IEmitConverter>();
    /// <summary>
    /// 成员缓存
    /// </summary>
    private IDictionary<Type, MemberBundle> _memberBundles = new ConcurrentDictionary<Type, MemberBundle>();
#else
    /// <summary>
    /// 转换器缓存
    /// </summary>
    private readonly ConcurrentDictionary<MapTypeKey, IEmitConverter> _converters = new();
        /// <summary>
    /// 转换器配置
    /// </summary>
    private readonly ConcurrentDictionary<MapTypeKey, IEmitConverter> _convertConfiguration = new();
    /// <summary>
    /// 成员缓存
    /// </summary>
    private readonly ConcurrentDictionary<Type, MemberBundle> _memberBundles = new();
#endif
    #endregion
    #region IConfigure<MapTypeKey, IEmitConverter>
    void IConfigure<MapTypeKey, IEmitConverter>.Configure(MapTypeKey key, IEmitConverter value)
        => _convertConfiguration[key] = value;
    /// <summary>
    /// 获取转化配置
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    internal virtual bool TryRead(MapTypeKey key, out IEmitConverter value)
        => _convertConfiguration.TryGetValue(key, out value);
    #endregion
    #region ISettings<MapTypeKey, IEmitTypeConverter>
    /// <inheritdoc />
    bool ICacher<MapTypeKey, IEmitConverter>.ContainsKey(MapTypeKey key)
        => _converters.ContainsKey(key);
    /// <inheritdoc />
    void IStore<MapTypeKey, IEmitConverter>.Set(MapTypeKey key, IEmitConverter value)
        => _converters[key] = value;
    /// <inheritdoc />
    bool ICacher<MapTypeKey, IEmitConverter>.TryGetValue(MapTypeKey key, out IEmitConverter value)
        => _converters.TryGetValue(key, out value) || TryRead(key, out value);
    #endregion
    #region ISettings<Type, MemberBundle>
    /// <inheritdoc />
    bool ICacher<Type, MemberBundle>.ContainsKey(Type key)
        => _memberBundles.ContainsKey(key);
    /// <inheritdoc />
    void IStore<Type, MemberBundle>.Set(Type key, MemberBundle value)
        => _memberBundles[key] = value;
    /// <inheritdoc />
    bool ICacher<Type, MemberBundle>.TryGetValue(Type key, out MemberBundle value)
        => _memberBundles.TryGetValue(key, out value);
    #endregion
#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    /// <summary>
    /// 设置为不可变
    /// </summary>
    public virtual void ToFrozen()
    {
        _converters = _converters.ToFrozenDictionary();
        _convertConfiguration = _convertConfiguration.ToFrozenDictionary();
        _memberBundles = _memberBundles.ToFrozenDictionary();
    }
#endif
}
