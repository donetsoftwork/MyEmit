using PocoEmit.Activators;
using PocoEmit.Builders;
using PocoEmit.Collections;
using PocoEmit.Converters;
using PocoEmit.Copies;
using PocoEmit.Maping;
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Configuration;

/// <summary>
/// 映射配置基类
/// </summary>
public abstract partial class MapperConfigurationBase
    : ConfigurationBase
    , IMapperOptions
{
    #region 配置
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
    /// 属性默认值配置
    /// </summary>
    private readonly ConcurrentDictionary<MemberInfo, IBuilder<Expression>> _memberDefaultValueConfiguration;
    /// <summary>
    /// 上下文转化缓存
    /// </summary>
    private readonly ConcurrentDictionary<PairTypeKey, IEmitContextConverter> _contextConverters;
    #endregion
    #region IMapperOptions
    /// <inheritdoc />
    public IMemberMatch GetMemberMatch(in PairTypeKey key)
    {
        _matchConfiguration.TryGetValue(key, out IMemberMatch matcher);
        return matcher ?? _defaultMatcher;
    }
    /// <inheritdoc />
    public Delegate GetCheckMembers(in PairTypeKey key)
    {
        _checkMembers.TryGetValue(key, out Delegate checker);
        return checker;
    }
    #region IConfigure<PairTypeKey, IEmitCopier>
    /// <inheritdoc />
    void IConfigure<PairTypeKey, IEmitCopier>.Configure(in PairTypeKey key, IEmitCopier config)
        => _copyConfiguration[key] = config;
    #endregion
    #region ICacher<PairTypeKey, IEmitCopier>
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IEmitCopier>.ContainsKey(in PairTypeKey key)
        => _copiers.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IEmitCopier>.TryGetCache(in PairTypeKey key, out IEmitCopier cached)
        => _copiers.TryGetValue(key, out cached) || _copyConfiguration.TryGetValue(key, out cached);
    /// <inheritdoc />
    void IStore<PairTypeKey, IEmitCopier>.Set(in PairTypeKey key, IEmitCopier value)
        => _copiers[key] = value;
    #endregion
    #region IConfigure<Type, IEmitActivator>
    /// <inheritdoc />
    void IConfigure<Type, IEmitActivator>.Configure(in Type key, IEmitActivator config)
        => _activeConfiguration[key] = config;
    #endregion
    #region IConfigure<PairTypeKey, IEmitActivator>
    /// <inheritdoc />
    void IConfigure<PairTypeKey, IEmitActivator>.Configure(in PairTypeKey key, IEmitActivator config)
        => _argumentActiveConfiguration[key] = config;
    #endregion
    #region IConfigure<PairTypeKey, Delegate>
    /// <inheritdoc />
    void IConfigure<PairTypeKey, Delegate>.Configure(in PairTypeKey key, Delegate config)
    {
        if (_checkMembers.TryGetValue(key, out var value0))
            config = Delegate.Combine(value0, config);
        _checkMembers[key] = config;
    }
    #endregion
    #region IConfigure<PairTypeKey, IMemberMatch>
    /// <inheritdoc />
    void IConfigure<PairTypeKey, IMemberMatch>.Configure(in PairTypeKey key, IMemberMatch config)
        => _matchConfiguration[key] = config;
    #endregion
    #region ICacher<Type, bool>
    /// <inheritdoc />
    bool ICacher<Type, bool>.ContainsKey(in Type key)
        => _primitiveTypes.ContainsKey(key);
    /// <inheritdoc />
    public virtual bool TryGetCache(in Type key, out bool cached)
        => _primitiveTypes.TryGetValue(key, out cached);
    /// <inheritdoc />
    void IStore<Type, bool>.Set(in Type key, bool value)
        => _primitiveTypes[key] = value;
    #endregion
    #region IConfigure<Type, bool>
    /// <inheritdoc />
    void IConfigure<Type, bool>.Configure(in Type key, bool config)
       => _primitiveTypes[key] = config;
    #endregion
    #region ISettings<Type, IBuilder<Expression>>
    #region IConfigure<Type, IBuilder<Expression>>
    /// <inheritdoc />
    void IConfigure<Type, IBuilder<Expression>>.Configure(in Type key, IBuilder<Expression> config)
        => _defaultValueConfiguration[key] = config;
    #endregion
    /// <inheritdoc />
    bool ISettings<Type, IBuilder<Expression>>.TryGetConfig(Type key, out IBuilder<Expression> config)
        => _defaultValueConfiguration.TryGetValue(key, out config);
    #endregion
    #region ISettings<MemberInfo, IBuilder<Expression>>
    #region IConfigure<MemberInfo, IBuilder<Expression>>
    /// <inheritdoc />
    void IConfigure<MemberInfo, IBuilder<Expression>>.Configure(in MemberInfo key, IBuilder<Expression> config)
        => _memberDefaultValueConfiguration[key] = config;
    #endregion
    /// <inheritdoc />
    bool ISettings<MemberInfo, IBuilder<Expression>>.TryGetConfig(MemberInfo key, out IBuilder<Expression> config)
        => _memberDefaultValueConfiguration.TryGetValue(key, out config);
    #endregion
    #region ICacher<PairTypeKey, IEmitContextConverter>
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IEmitContextConverter>.ContainsKey(in PairTypeKey key)
        => _contextConverters.ContainsKey(key);
    /// <inheritdoc />
    void IStore<PairTypeKey, IEmitContextConverter>.Set(in PairTypeKey key, IEmitContextConverter value)
        => _contextConverters[key] = value;
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IEmitContextConverter>.TryGetCache(in PairTypeKey key, out IEmitContextConverter cached)
        => _contextConverters.TryGetValue(key, out cached);
    #endregion
    #endregion
}
