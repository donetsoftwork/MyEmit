using PocoEmit.Activators;
using PocoEmit.Collections;
using PocoEmit.Copies;
using PocoEmit.Maping;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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
    /// 激活器缓存
    /// </summary>
    private IDictionary<Type, IEmitActivator> _activators = new ConcurrentDictionary<Type, IEmitActivator>();
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
    /// 激活器缓存
    /// </summary>
    private ConcurrentDictionary<Type, IEmitActivator> _activators = new();
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
    /// <summary>
    /// 复制器
    /// </summary>
    public IEnumerable<IEmitCopier> Copiers
        => _copiers.Values;
    /// <summary>
    /// 成员匹配配置
    /// </summary>
    public IEnumerable<IMemberMatch> MatchConfiguration
        => _matchConfiguration.Values.Concat([_defaultMatcher]).Distinct();
    /// <summary>
    /// 基础类型
    /// </summary>
    public IEnumerable<Type> PrimitiveTypes
        => _primitiveTypes.Where(p => p.Value).Select(p => p.Key);
    /// <inheritdoc />
    public IMemberMatch GetMemberMatch(MapTypeKey key)
    {
        TryRead(key, out IMemberMatch matcher);
        return matcher ?? _defaultMatcher;
    }
    #region IConfigure<MapTypeKey, IEmitCopier>
    /// <inheritdoc />
    void IConfigure<MapTypeKey, IEmitCopier>.Configure(MapTypeKey key, IEmitCopier value)
        => _copyConfiguration[key] = value;
    /// <summary>
    /// 获取复制配置
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    internal virtual bool TryRead(MapTypeKey key, out IEmitCopier value)
        => _copyConfiguration.TryGetValue(key, out value);
    #endregion
    #region ISettings<MapTypeKey, IEmitCopier>
    /// <inheritdoc />
    bool ICacher<MapTypeKey, IEmitCopier>.ContainsKey(MapTypeKey key)
        => _copiers.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<MapTypeKey, IEmitCopier>.TryGetValue(MapTypeKey key, out IEmitCopier value)
        => _copiers.TryGetValue(key, out value) || TryRead(key, out value);
    /// <inheritdoc />
    void IStore<MapTypeKey, IEmitCopier>.Set(MapTypeKey key, IEmitCopier value)
        => _copiers[key] = value;
    #endregion
    #region IConfigure<Type, IEmitActivator>
    void IConfigure<Type, IEmitActivator>.Configure(Type key, IEmitActivator value)
        => _activeConfiguration[key] = value;
    /// <summary>
    /// 获取激活配置
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    internal virtual bool TryRead(Type key, out IEmitActivator value)
        => _activeConfiguration.TryGetValue(key, out value);
    #endregion
    #region ISettings<Type, IEmitActivator>
    /// <inheritdoc />
    bool ICacher<Type, IEmitActivator>.ContainsKey(Type key)
        => _activators.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<Type, IEmitActivator>.TryGetValue(Type key, out IEmitActivator value)
        => _activators.TryGetValue(key, out value) || TryRead(key, out value);
    /// <inheritdoc />
    void IStore<Type, IEmitActivator>.Set(Type key, IEmitActivator value)
        => _activators[key] = value;
    #endregion
    #region IConfigure<MapTypeKey, IEmitActivator>
    void IConfigure<MapTypeKey, IEmitActivator>.Configure(MapTypeKey key, IEmitActivator value)
        => _argumentActiveConfiguration[key] = value;
    /// <summary>
    /// 获取带参激活配置
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    internal virtual bool TryRead(MapTypeKey key, out IEmitActivator value)
        => _argumentActiveConfiguration.TryGetValue(key, out value);
    #endregion
    #region IConfigure<MapTypeKey, IMemberMatch>
    /// <summary>
    /// 获取匹配规则
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    internal virtual bool TryRead(MapTypeKey key, out IMemberMatch value)
        => _matchConfiguration.TryGetValue(key, out value);
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
    /// <summary>
    /// 读取默认值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    internal virtual bool TryRead(Type key, out object value)
        => _defaultValueConfiguration.TryGetValue(key, out value);
    /// <inheritdoc />
    void IConfigure<Type, object>.Configure(Type key, object value)
        => _defaultValueConfiguration[key] = value;
    #endregion
    /// <inheritdoc />
    public Expression CreateDefault(Type destType)
    {
        if (TryRead(destType, out object defaultValue))
        {
            if (defaultValue is Delegate func)
            {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
                var call = Expression.Call(ReflectionHelper.CheckMethodCallInstance(func), func.GetMethodInfo());
#else
                var call = Expression.Call(ReflectionHelper.CheckMethodCallInstance(func), func.Method);
#endif
                return Expression.Convert(call, destType);
            }
            else
            {
                return Expression.Constant(defaultValue);
            }
        }
        else
        {
            return Expression.Default(destType);
        }
    }
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
        _activeConfiguration = _activeConfiguration.ToFrozenDictionary();
        _argumentActiveConfiguration = _argumentActiveConfiguration.ToFrozenDictionary();
        _matchConfiguration = _matchConfiguration.ToFrozenDictionary();
        _primitiveTypes = _primitiveTypes.ToFrozenDictionary();
    }
#endif
}
