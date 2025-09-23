using PocoEmit.Activators;
using PocoEmit.Collections;
using PocoEmit.Copies;
using PocoEmit.Maping;
using System;
using PocoEmit.Builders;
using System.Linq.Expressions;
using PocoEmit.Converters;
using System.Collections.Concurrent;

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
    /// 上下文转化缓存
    /// </summary>
    private readonly ConcurrentDictionary<PairTypeKey, IContextConverter> _contextConverters;
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
    void IConfigure<PairTypeKey, IEmitCopier>.Configure(in PairTypeKey key, IEmitCopier value)
        => _copyConfiguration[key] = value;
    #endregion
    #region ICacher<PairTypeKey, IEmitCopier>
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IEmitCopier>.ContainsKey(in PairTypeKey key)
        => _copiers.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IEmitCopier>.TryGetValue(in PairTypeKey key, out IEmitCopier value)
        => _copiers.TryGetValue(key, out value) || _copyConfiguration.TryGetValue(key, out value);
    /// <inheritdoc />
    void IStore<PairTypeKey, IEmitCopier>.Set(in PairTypeKey key, IEmitCopier value)
        => _copiers[key] = value;
    #endregion
    #region IConfigure<Type, IEmitActivator>
    /// <inheritdoc />
    void IConfigure<Type, IEmitActivator>.Configure(in Type key, IEmitActivator value)
        => _activeConfiguration[key] = value;
    #endregion
    #region IConfigure<PairTypeKey, IEmitActivator>
    /// <inheritdoc />
    void IConfigure<PairTypeKey, IEmitActivator>.Configure(in PairTypeKey key, IEmitActivator value)
        => _argumentActiveConfiguration[key] = value;
    #endregion
    #region IConfigure<PairTypeKey, Delegate>
    /// <inheritdoc />
    void IConfigure<PairTypeKey, Delegate>.Configure(in PairTypeKey key, Delegate value)
    {
        if (_checkMembers.TryGetValue(key, out var value0))
            value = Delegate.Combine(value0, value);
        _checkMembers[key] = value;
    }
    #endregion
    #region IConfigure<PairTypeKey, IMemberMatch>
    /// <inheritdoc />
    void IConfigure<PairTypeKey, IMemberMatch>.Configure(in PairTypeKey key, IMemberMatch value)
        => _matchConfiguration[key] = value;
    #endregion
    #region ICacher<Type, bool>
    /// <inheritdoc />
    bool ICacher<Type, bool>.ContainsKey(in Type key)
        => _primitiveTypes.ContainsKey(key);
    /// <inheritdoc />
    public virtual bool TryGetValue(in Type key, out bool value)
        => _primitiveTypes.TryGetValue(key, out value);
    /// <inheritdoc />
    void IStore<Type, bool>.Set(in Type key, bool value)
        => _primitiveTypes[key] = value;
    #endregion
    #region IConfigure<Type, bool>
    /// <inheritdoc />
    void IConfigure<Type, bool>.Configure(in Type key, bool value)
       => _primitiveTypes[key] = value;
    #endregion
    #region IConfigure<Type, IBuilder<Expression>>
    /// <inheritdoc />
    void IConfigure<Type, IBuilder<Expression>>.Configure(in Type key, IBuilder<Expression> value)
        => _defaultValueConfiguration[key] = value;
    #endregion
    #region ICacher<PairTypeKey, IContextConverter>
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IContextConverter>.ContainsKey(in PairTypeKey key)
        => _contextConverters.ContainsKey(key);
    /// <inheritdoc />
    void IStore<PairTypeKey, IContextConverter>.Set(in PairTypeKey key, IContextConverter value)
        => _contextConverters[key] = value;
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IContextConverter>.TryGetValue(in PairTypeKey key, out IContextConverter value)
        => _contextConverters.TryGetValue(key, out value);
    #endregion
    #endregion
}
