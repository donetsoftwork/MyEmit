using PocoEmit.Activators;
using PocoEmit.Collections;
using PocoEmit.Copies;
using PocoEmit.Maping;
using System;
using System.Collections.Concurrent;

#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
using System.Collections.Generic;
using System.Collections.Frozen;
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
#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    /// <summary>
    /// 复制器缓存
    /// </summary>
    private IDictionary<MapTypeKey, IEmitCopier> _copiers = new ConcurrentDictionary<MapTypeKey, IEmitCopier>();
    /// <summary>
    /// 复制器配置
    /// </summary>
    private IDictionary<MapTypeKey, IEmitCopier> _copyConfiguration = new ConcurrentDictionary<MapTypeKey, IEmitCopier>();
    /// <summary>
    /// 激活器配置
    /// </summary>
    private IDictionary<Type, IEmitActivator> _activeConfiguration = new ConcurrentDictionary<Type, IEmitActivator>();
    /// <summary>
    /// 带参激活器配置
    /// </summary>
    private IDictionary<MapTypeKey, IEmitActivator> _argumentActiveConfiguration = new ConcurrentDictionary<MapTypeKey, IEmitActivator>();
    /// <summary>
    /// 成员匹配配置
    /// </summary>
    private IDictionary<MapTypeKey, IMemberMatch> _matchConfiguration = new ConcurrentDictionary<MapTypeKey, IMemberMatch>();
    /// <summary>
    /// 基础类型配置
    /// </summary>
    private IDictionary<Type, bool> _primitiveTypes = new ConcurrentDictionary<Type, bool>();
    /// <summary>
    /// 默认值配置
    /// </summary>
    private IDictionary<Type, object> _defaultValueConfiguration = new ConcurrentDictionary<Type, object>();
#else
    /// <summary>
    /// 复制器缓存
    /// </summary>
    private ConcurrentDictionary<MapTypeKey, IEmitCopier> _copiers = new();
    /// <summary>
    /// 复制器配置
    /// </summary>
    private ConcurrentDictionary<MapTypeKey, IEmitCopier> _copyConfiguration= new();
    /// <summary>
    /// 激活器配置
    /// </summary>
    private ConcurrentDictionary<Type, IEmitActivator> _activeConfiguration = new();
    /// <summary>
    /// 带参激活器配置
    /// </summary>
    private ConcurrentDictionary<MapTypeKey, IEmitActivator> _argumentActiveConfiguration = new();
    /// <summary>
    /// 成员匹配配置
    /// </summary>
    private ConcurrentDictionary<MapTypeKey, IMemberMatch> _matchConfiguration = new();
    /// <summary>
    /// 基础类型配置
    /// </summary>
    private ConcurrentDictionary<Type, bool> _primitiveTypes = new();
    /// <summary>
    /// 默认值配置
    /// </summary>
    private ConcurrentDictionary<Type, object> _defaultValueConfiguration = new();
#endif
    #endregion
    #region IMapperOptions
    /// <inheritdoc />
    public IMemberMatch GetMemberMatch(MapTypeKey key)
    {
        _matchConfiguration.TryGetValue(key, out IMemberMatch matcher);
        return matcher ?? _defaultMatcher;
    }
    #region IConfigure<MapTypeKey, IEmitCopier>
    /// <inheritdoc />
    void IConfigure<MapTypeKey, IEmitCopier>.Configure(MapTypeKey key, IEmitCopier value)
        => _copyConfiguration[key] = value;
    #endregion
    #region ISettings<MapTypeKey, IEmitCopier>
    /// <inheritdoc />
    bool ICacher<MapTypeKey, IEmitCopier>.ContainsKey(MapTypeKey key)
        => _copiers.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<MapTypeKey, IEmitCopier>.TryGetValue(MapTypeKey key, out IEmitCopier value)
        => _copiers.TryGetValue(key, out value) || _copyConfiguration.TryGetValue(key, out value);
    /// <inheritdoc />
    void IStore<MapTypeKey, IEmitCopier>.Set(MapTypeKey key, IEmitCopier value)
        => _copiers[key] = value;
    #endregion
    #region IConfigure<Type, IEmitActivator>
    /// <inheritdoc />
    void IConfigure<Type, IEmitActivator>.Configure(Type key, IEmitActivator value)
        => _activeConfiguration[key] = value;
    #endregion
    #region IConfigure<MapTypeKey, IEmitActivator>
    /// <inheritdoc />
    void IConfigure<MapTypeKey, IEmitActivator>.Configure(MapTypeKey key, IEmitActivator value)
        => _argumentActiveConfiguration[key] = value;
    #endregion
    #region IConfigure<MapTypeKey, IMemberMatch>
    /// <inheritdoc />
    void IConfigure<MapTypeKey, IMemberMatch>.Configure(MapTypeKey key, IMemberMatch value)
        => _matchConfiguration[key] = value;
    #endregion
    #region ISettings<Type, bool>
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
    #region IConfigure<Type, object>
    /// <inheritdoc />
    void IConfigure<Type, object>.Configure(Type key, object value)
        => _defaultValueConfiguration[key] = value;
    #endregion
    #endregion
#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    /// <summary>
    /// 设置为不可变
    /// </summary>
    public override void ToFrozen()
    {
        base.ToFrozen();
        _copiers = _copiers.ToFrozenDictionary();
        _activeConfiguration = _activeConfiguration.ToFrozenDictionary();
        _argumentActiveConfiguration = _argumentActiveConfiguration.ToFrozenDictionary();
        _matchConfiguration = _matchConfiguration.ToFrozenDictionary();        
        _primitiveTypes = _primitiveTypes.ToFrozenDictionary();
    }
#endif
}
