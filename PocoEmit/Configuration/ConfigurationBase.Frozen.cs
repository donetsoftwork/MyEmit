using PocoEmit.Converters;
using System.Collections.Concurrent;
using PocoEmit.Collections;
using System;
using System.Reflection;
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
    /// 转换器配置(优先)
    /// </summary>
    private IDictionary<MapTypeKey, IEmitConverter> _convertSetting = new ConcurrentDictionary<MapTypeKey, IEmitConverter>();
    /// <summary>
    /// 成员缓存
    /// </summary>
    private IDictionary<Type, MemberBundle> _memberBundles = new ConcurrentDictionary<Type, MemberBundle>();
    /// <summary>
    /// 成员读委托缓存
    /// </summary>
    private IDictionary<MemberInfo, Func<object, object>> _readFuncs = new ConcurrentDictionary<MemberInfo, Func<object, object>>();
    /// <summary>
    /// 成员写委托缓存
    /// </summary>
    private IDictionary<MemberInfo, Action<object, object>> _writeActions = new ConcurrentDictionary<MemberInfo, Action<object, object>>();
#else
    /// <summary>
    /// 转换器缓存
    /// </summary>
    private readonly ConcurrentDictionary<MapTypeKey, IEmitConverter> _converters = new();
        /// <summary>
    /// 转换器配置(优先)
    /// </summary>
    private readonly ConcurrentDictionary<MapTypeKey, IEmitConverter> _convertSetting = new();
    /// <summary>
    /// 成员缓存
    /// </summary>
    private readonly ConcurrentDictionary<Type, MemberBundle> _memberBundles = new();
        /// <summary>
    /// 成员读委托缓存
    /// </summary>
    private readonly ConcurrentDictionary<MemberInfo, Func<object, object>> _readFuncs = new();
    /// <summary>
    /// 成员写委托缓存
    /// </summary>
    private readonly ConcurrentDictionary<MemberInfo, Action<object, object>> _writeActions = new();
#endif
    #endregion
    /// <summary>
    /// 获取转化配置
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public virtual bool TryGetConvertSetting(MapTypeKey key, out IEmitConverter value)
        => _convertSetting.TryGetValue(key, out value);
    #region ISettings<MapTypeKey, IEmitTypeConverter>
    /// <inheritdoc />
    bool ISettings<MapTypeKey, IEmitConverter>.ContainsKey(MapTypeKey key)
        => _converters.ContainsKey(key);
    /// <inheritdoc />
    public void Set(MapTypeKey key, IEmitConverter value)
        => _converters[key] = value;
    /// <inheritdoc />
    bool ISettings<MapTypeKey, IEmitConverter>.TryGetValue(MapTypeKey key, out IEmitConverter value)
        => _converters.TryGetValue(key, out value) || TryGetConvertSetting(key, out value);
    #endregion
    #region ISettings<Type, MemberBundle>
    /// <inheritdoc />
    bool ISettings<Type, MemberBundle>.ContainsKey(Type key)
        => _memberBundles.ContainsKey(key);
    /// <inheritdoc />
    void ISettings<Type, MemberBundle>.Set(Type key, MemberBundle value)
        => _memberBundles[key] = value;
    /// <inheritdoc />
    bool ISettings<Type, MemberBundle>.TryGetValue(Type key, out MemberBundle value)
        => _memberBundles.TryGetValue(key, out value);
    #endregion
    #region ISettings<MemberInfo, Func<object, object>>
    /// <inheritdoc />
    bool ISettings<MemberInfo, Func<object, object>>.ContainsKey(MemberInfo key)
        => _readFuncs.ContainsKey(key);
    /// <inheritdoc />
    void ISettings<MemberInfo, Func<object, object>>.Set(MemberInfo key, Func<object, object> value)
        => _readFuncs[key] = value;
    /// <inheritdoc />
    bool ISettings<MemberInfo, Func<object, object>>.TryGetValue(MemberInfo key, out Func<object, object> value)
        => _readFuncs.TryGetValue(key, out value);
    #endregion
    #region ISettings<MemberInfo, Action<object, object>>
    /// <inheritdoc />
    bool ISettings<MemberInfo, Action<object, object>>.ContainsKey(MemberInfo key)
        => _writeActions.ContainsKey(key);
    /// <inheritdoc />
    void ISettings<MemberInfo, Action<object, object>>.Set(MemberInfo key, Action<object, object> value)
        => _writeActions[key] = value;
    /// <inheritdoc />
    bool ISettings<MemberInfo, Action<object, object>>.TryGetValue(MemberInfo key, out Action<object, object> value)
        => _writeActions.TryGetValue(key, out value);
    #endregion
#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    /// <summary>
    /// 设置为不可变
    /// </summary>
    public virtual void ToFrozen()
    {
        _converters = _converters.ToFrozenDictionary();
        _convertSetting = _convertSetting.ToFrozenDictionary();
        _memberBundles = _memberBundles.ToFrozenDictionary();
        _readFuncs = _readFuncs.ToFrozenDictionary();
        _writeActions = _writeActions.ToFrozenDictionary();
    }
#endif
}
