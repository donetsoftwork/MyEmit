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
    private IDictionary<PairTypeKey, IEmitConverter> _converters;
    /// <summary>
    /// 转换器配置
    /// </summary>
    private IDictionary<PairTypeKey, IEmitConverter> _convertConfiguration;
    /// <summary>
    /// 成员缓存
    /// </summary>
    private IDictionary<Type, MemberBundle> _memberBundles;
#else
    /// <summary>
    /// 转换器缓存
    /// </summary>
    private readonly ConcurrentDictionary<PairTypeKey, IEmitConverter> _converters;
        /// <summary>
    /// 转换器配置
    /// </summary>
    private readonly ConcurrentDictionary<PairTypeKey, IEmitConverter> _convertConfiguration;
    /// <summary>
    /// 成员缓存
    /// </summary>
    private readonly ConcurrentDictionary<Type, MemberBundle> _memberBundles;
#endif
    #endregion
    #region IConfigure<PairTypeKey, IEmitConverter>
    void IConfigure<PairTypeKey, IEmitConverter>.Configure(PairTypeKey key, IEmitConverter value)
        => _convertConfiguration[key] = value;
    #endregion
    #region ISettings<PairTypeKey, IEmitTypeConverter>
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IEmitConverter>.ContainsKey(PairTypeKey key)
        => _converters.ContainsKey(key);
    /// <inheritdoc />
    void IStore<PairTypeKey, IEmitConverter>.Set(PairTypeKey key, IEmitConverter value)
        => _converters[key] = value;
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IEmitConverter>.TryGetValue(PairTypeKey key, out IEmitConverter value)
        => _converters.TryGetValue(key, out value) || _convertConfiguration.TryGetValue(key, out value);
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
