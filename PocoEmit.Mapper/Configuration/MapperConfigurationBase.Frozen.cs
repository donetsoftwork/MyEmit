using PocoEmit.Activators;
using PocoEmit.Collections;
using PocoEmit.Copies;
using PocoEmit.Maping;
using System;
using PocoEmit.Builders;
using System.Linq.Expressions;
using PocoEmit.Converters;


#if NET8_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
using System.Collections.Generic;
using System.Collections.Frozen;
#else
using System.Collections.Concurrent;
#endif

namespace PocoEmit.Configuration;

/// <summary>
/// 映射配置基类
/// </summary>
public abstract partial class MapperConfigurationBase
    : ConfigurationBase
    , IMapperOptions
{
    #region 配置
#if NET8_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    /// <summary>
    /// 复制器缓存
    /// </summary>
    private IDictionary<PairTypeKey, IEmitCopier> _copiers;
    /// <summary>
    /// 复制器配置
    /// </summary>
    private IDictionary<PairTypeKey, IEmitCopier> _copyConfiguration;
    /// <summary>
    /// 激活器配置
    /// </summary>
    private IDictionary<Type, IEmitActivator> _activeConfiguration;
    /// <summary>
    /// 带参激活器配置
    /// </summary>
    private IDictionary<PairTypeKey, IEmitActivator> _argumentActiveConfiguration;
    /// <summary>
    /// 转化后成员检查配置
    /// </summary>
    private IDictionary<PairTypeKey, Delegate> _checkMembers;
    /// <summary>
    /// 成员匹配配置
    /// </summary>
    private IDictionary<PairTypeKey, IMemberMatch> _matchConfiguration;
    /// <summary>
    /// 基础类型配置
    /// </summary>
    private IDictionary<Type, bool> _primitiveTypes;
    /// <summary>
    /// 默认值配置
    /// </summary>
    private IDictionary<Type, IBuilder<Expression>> _defaultValueConfiguration;
        /// <summary>
    /// 上下文转化缓存
    /// </summary>
    private IDictionary<PairTypeKey, IContextConverter> _contextConverters;
#else
    /// <summary>
    /// 复制器缓存
    /// </summary>
    private readonly ConcurrentDictionary<PairTypeKey, IEmitCopier> _copiers;
    /// <summary>
    /// 复制器配置
    /// </summary>
    private readonly ConcurrentDictionary<PairTypeKey, IEmitCopier> _copyConfiguration;
    /// <summary>
    /// 激活器配置
    /// </summary>
    private readonly ConcurrentDictionary<Type, IEmitActivator> _activeConfiguration;
    /// <summary>
    /// 带参激活器配置
    /// </summary>
    private readonly ConcurrentDictionary<PairTypeKey, IEmitActivator> _argumentActiveConfiguration;
    /// <summary>
    /// 转化后成员检查配置
    /// </summary>
    private readonly ConcurrentDictionary<PairTypeKey, Delegate> _checkMembers;
    /// <summary>
    /// 成员匹配配置
    /// </summary>
    private readonly ConcurrentDictionary<PairTypeKey, IMemberMatch> _matchConfiguration;
    /// <summary>
    /// 基础类型配置
    /// </summary>
    private readonly ConcurrentDictionary<Type, bool> _primitiveTypes;
    /// <summary>
    /// 默认值配置
    /// </summary>
    private readonly ConcurrentDictionary<Type, IBuilder<Expression>> _defaultValueConfiguration;
    /// <summary>
    /// 上下文转化缓存
    /// </summary>
    private readonly ConcurrentDictionary<PairTypeKey, IContextConverter> _contextConverters;
#endif
    #endregion
    #region IMapperOptions
    /// <inheritdoc />
    public IMemberMatch GetMemberMatch(PairTypeKey key)
    {
        _matchConfiguration.TryGetValue(key, out IMemberMatch matcher);
        return matcher ?? _defaultMatcher;
    }
    /// <inheritdoc />
    public Delegate GetCheckMembers(PairTypeKey key)
    {
        _checkMembers.TryGetValue(key, out Delegate checker);
        return checker;
    }
    #region IConfigure<PairTypeKey, IEmitCopier>
    /// <inheritdoc />
    void IConfigure<PairTypeKey, IEmitCopier>.Configure(PairTypeKey key, IEmitCopier value)
        => _copyConfiguration[key] = value;
    #endregion
    #region ICacher<PairTypeKey, IEmitCopier>
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IEmitCopier>.ContainsKey(PairTypeKey key)
        => _copiers.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IEmitCopier>.TryGetValue(PairTypeKey key, out IEmitCopier value)
        => _copiers.TryGetValue(key, out value) || _copyConfiguration.TryGetValue(key, out value);
    /// <inheritdoc />
    void IStore<PairTypeKey, IEmitCopier>.Set(PairTypeKey key, IEmitCopier value)
        => _copiers[key] = value;
    #endregion
    #region IConfigure<Type, IEmitActivator>
    /// <inheritdoc />
    void IConfigure<Type, IEmitActivator>.Configure(Type key, IEmitActivator value)
        => _activeConfiguration[key] = value;
    #endregion
    #region IConfigure<PairTypeKey, IEmitActivator>
    /// <inheritdoc />
    void IConfigure<PairTypeKey, IEmitActivator>.Configure(PairTypeKey key, IEmitActivator value)
        => _argumentActiveConfiguration[key] = value;
    #endregion
    #region IConfigure<PairTypeKey, Delegate>
    /// <inheritdoc />
    void IConfigure<PairTypeKey, Delegate>.Configure(PairTypeKey key, Delegate value)
    {
        if (_checkMembers.TryGetValue(key, out var value0))
            value = Delegate.Combine(value0, value);
        _checkMembers[key] = value;
    }
    #endregion
    #region IConfigure<PairTypeKey, IMemberMatch>
    /// <inheritdoc />
    void IConfigure<PairTypeKey, IMemberMatch>.Configure(PairTypeKey key, IMemberMatch value)
        => _matchConfiguration[key] = value;
    #endregion
    #region ICacher<Type, bool>
    /// <inheritdoc />
    bool ICacher<Type, bool>.ContainsKey(Type key)
        => _primitiveTypes.ContainsKey(key);
    /// <inheritdoc />
    public virtual bool TryGetValue(Type key, out bool value)
        => _primitiveTypes.TryGetValue(key, out value);
    /// <inheritdoc />
    void IStore<Type, bool>.Set(Type key, bool value)
        => _primitiveTypes[key] = value;
    #endregion
    #region IConfigure<Type, bool>
    /// <inheritdoc />
    void IConfigure<Type, bool>.Configure(Type key, bool value)
       => _primitiveTypes[key] = value;
    #endregion
    #region IConfigure<Type, IBuilder<Expression>>
    /// <inheritdoc />
    void IConfigure<Type, IBuilder<Expression>>.Configure(Type key, IBuilder<Expression> value)
        => _defaultValueConfiguration[key] = value;
    #endregion
    #region ICacher<PairTypeKey, IContextConverter>
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IContextConverter>.ContainsKey(PairTypeKey key)
        => _contextConverters.ContainsKey(key);
    /// <inheritdoc />
    void IStore<PairTypeKey, IContextConverter>.Set(PairTypeKey key, IContextConverter value)
        => _contextConverters[key] = value;
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IContextConverter>.TryGetValue(PairTypeKey key, out IContextConverter value)
        => _contextConverters.TryGetValue(key, out value);
    #endregion
    #endregion
#if NET8_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    /// <summary>
    /// 设置为不可变
    /// </summary>
    public override void ToFrozen()
    {
        base.ToFrozen();
        _copiers = _copiers.ToFrozenDictionary();
        _contextConverters = _contextConverters.ToFrozenDictionary();
    }
#endif
}
