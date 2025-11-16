using PocoEmit.Converters;
using PocoEmit.Collections;
using System;
using System.Collections.Concurrent;
using Hand.Cache;
using Hand.Collections;
using Hand.Reflection;
using Hand.Configuration;

namespace PocoEmit.Configuration;

/// <summary>
/// Emit配置
/// </summary>
public abstract partial class ConfigurationBase
    : IPocoOptions
{
    #region 缓存数据
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
    #endregion
    #region IConfigure<PairTypeKey, IEmitConverter>
    void IConfigure<PairTypeKey, IEmitConverter>.Set(in PairTypeKey key, IEmitConverter config)
        => _convertConfiguration[key] = config;
    #endregion
    #region ICacher<PairTypeKey, IEmitConverter>
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IEmitConverter>.ContainsKey(in PairTypeKey key)
        => _converters.ContainsKey(key);
    /// <inheritdoc />
    void IStore<PairTypeKey, IEmitConverter>.Save(in PairTypeKey key, IEmitConverter value)
        => _converters[key] = value;
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IEmitConverter>.TryGetCache(in PairTypeKey key, out IEmitConverter cached)
        => _converters.TryGetValue(key, out cached) || _convertConfiguration.TryGetValue(key, out cached);
    #endregion
    #region ICacher<Type, MemberBundle>
    /// <inheritdoc />
    bool ICacher<Type, MemberBundle>.ContainsKey(in Type key)
        => _memberBundles.ContainsKey(key);
    /// <inheritdoc />
    void IStore<Type, MemberBundle>.Save(in Type key, MemberBundle value)
        => _memberBundles[key] = value;
    /// <inheritdoc />
    bool ICacher<Type, MemberBundle>.TryGetCache(in Type key, out MemberBundle cached)
        => _memberBundles.TryGetValue(key, out cached);
    #endregion
}
