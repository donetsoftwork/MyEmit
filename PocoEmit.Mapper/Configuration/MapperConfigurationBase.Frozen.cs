using PocoEmit.Activators;
using PocoEmit.Collections;
using PocoEmit.Copies;
using PocoEmit.Maping;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
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
    private IDictionary<MapTypeKey, IEmitCopier> _copiers = new ConcurrentDictionary<MapTypeKey, IEmitCopier>();
    private IDictionary<Type, IEmitActivator> _activators = new ConcurrentDictionary<Type, IEmitActivator>();
    internal IDictionary<MapTypeKey, IMemberMatch> _matches = new ConcurrentDictionary<MapTypeKey, IMemberMatch>();
    private IDictionary<Type, bool> _primitiveTypes = new ConcurrentDictionary<Type, bool>();
    private IDictionary<Type, object> _defaultValues = new ConcurrentDictionary<Type, object>();
    #endregion
    #region IMapperOptions
    /// <inheritdoc />
    public IEnumerable<IEmitCopier> Copiers
        => _copiers.Values;
    /// <inheritdoc />
    public IEnumerable<IMemberMatch> Matches
        => _matches.Values.Concat([_defaultMatch]).Distinct();
    /// <inheritdoc />
    public IEnumerable<Type> PrimitiveTypes
        => _primitiveTypes.Where(p => p.Value).Select(p => p.Key);
    /// <inheritdoc />
    public virtual IMemberMatch GetMemberMatch(MapTypeKey key)
    {
        _matches.TryGetValue(key, out IMemberMatch match);
        return match ?? _defaultMatch;
    }
    #region ISettings<MapTypeKey, IEmitCopier>
    /// <inheritdoc />
    bool ISettings<MapTypeKey, IEmitCopier>.ContainsKey(MapTypeKey key)
        => _copiers.ContainsKey(key);
    /// <inheritdoc />
    bool ISettings<MapTypeKey, IEmitCopier>.TryGetValue(MapTypeKey key, out IEmitCopier value)
        => _copiers.TryGetValue(key, out value);
    /// <inheritdoc />
    public void Set(MapTypeKey key, IEmitCopier value)
        => _copiers[key] = value;
    #endregion
    #region ISettings<Type, IEmitActivator>
    /// <inheritdoc />
    bool ISettings<Type, IEmitActivator>.ContainsKey(Type key)
        => _activators.ContainsKey(key);
    /// <inheritdoc />
    bool ISettings<Type, IEmitActivator>.TryGetValue(Type key, out IEmitActivator value)
        => _activators.TryGetValue(key, out value);
    /// <inheritdoc />
    void ISettings<Type, IEmitActivator>.Set(Type key, IEmitActivator value)
        => _activators[key] = value;
    #endregion
    #region ISettings<MapTypeKey, IMemberMatch>
    /// <inheritdoc />
    bool ISettings<MapTypeKey, IMemberMatch>.ContainsKey(MapTypeKey key)
        => _matches.ContainsKey(key);
    /// <inheritdoc />
    public virtual bool TryGetValue(MapTypeKey key, out IMemberMatch value)
        => _matches.TryGetValue(key, out value);
    /// <inheritdoc />
    void ISettings<MapTypeKey, IMemberMatch>.Set(MapTypeKey key, IMemberMatch value)
        => _matches[key] = value;
    #endregion
    #region ISettings<Type, bool>
    /// <inheritdoc />
    bool ISettings<Type, bool>.ContainsKey(Type key)
        => _primitiveTypes.ContainsKey(key);
    /// <inheritdoc />
    public virtual bool TryGetValue(Type key, out bool value)
        => _primitiveTypes.TryGetValue(key, out value);
    /// <inheritdoc />
    void ISettings<Type, bool>.Set(Type key, bool value)
        => _primitiveTypes[key] = value;
    #endregion
    #region ISettings<Type, object>
    /// <inheritdoc />
    bool ISettings<Type, object>.ContainsKey(Type key)
        => _defaultValues.ContainsKey(key);
    /// <inheritdoc />
    bool ISettings<Type, object>.TryGetValue(Type key, out object value)
        => _defaultValues.TryGetValue(key, out value);
    /// <inheritdoc />
    void ISettings<Type, object>.Set(Type key, object value)
        => _defaultValues[key] = value;
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
        _activators = _activators.ToFrozenDictionary();
        _matches = _matches.ToFrozenDictionary();
        _primitiveTypes = _primitiveTypes.ToFrozenDictionary();
    }
#endif
}
