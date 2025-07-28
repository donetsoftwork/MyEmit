using PocoEmit.Activators;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Copies;
using PocoEmit.Maping;
using PocoEmit.Reflection;
using System;

namespace PocoEmit;

/// <summary>
/// 映射配置
/// </summary>
public sealed class Mapper(IReflectionMember reflection)
    : MapperConfigurationBase(reflection)
{
    /// <summary>
    /// Emit配置
    /// </summary>
    public Mapper()
        : this(DefaultReflect)
    {
    }
    #region IMapperOptions
    #region IEmitOptions
    /// <inheritdoc />
    public override bool TryGetValue(MapTypeKey key, out IEmitConverter value)
    {
        return base.TryGetValue(key, out value)
            || GlobalOptions.Instance.TryGetValue(key, out value);
    }
    #endregion
    /// <inheritdoc />
    public override IMemberMatch GetMemberMatch(MapTypeKey key)
    {
        if (_matches.TryGetValue(key, out IMemberMatch match) || GlobalOptions.Instance._matches.TryGetValue(key, out match))
            return match;
        return _defaultMatch;
    }
    /// <inheritdoc />
    public override bool TryGetValue(MapTypeKey key, out IEmitCopier value)
    {
        return base.TryGetValue(key, out value)
            || GlobalOptions.Instance.TryGetValue(key, out value);
    }
    /// <inheritdoc />
    public override bool TryGetValue(MapTypeKey key, out IMemberMatch value)
    {
        return base.TryGetValue(key, out value)
            || GlobalOptions.Instance.TryGetValue(key, out value);
    }
    /// <inheritdoc />
    public override bool TryGetValue(Type key, out IEmitActivator value)
    {
        return base.TryGetValue(key, out value)
            || GlobalOptions.Instance.TryGetValue(key, out value);
    }
    /// <inheritdoc />
    public override bool TryGetValue(Type key, out bool value)
    {
        return base.TryGetValue(key, out value)
            || GlobalOptions.Instance.TryGetValue(key, out value);
    }
    #endregion
    #region Global
    private static IReflectionMember _defaultReflect = DefaultReflectionMember.Default;
    /// <summary>
    /// 默认反射成员对象
    /// </summary>
    public static IReflectionMember DefaultReflect
    {
        get => _defaultReflect;
        set
        {
            _defaultReflect = value ?? throw new ArgumentNullException(nameof(value));
            GlobalOptions.Instance.Reflection = value;
        }
    }
    /// <summary>
    /// Emit配置
    /// </summary>
    public static IMapperOptions Global
        => GlobalOptions.Instance;
    /// <summary>
    /// 全局配置
    /// </summary>
    sealed class GlobalOptions()
        : MapperConfigurationBase(DefaultReflect)
    {
        /// <summary>
        /// Emit全局配置
        /// </summary>
        public static readonly GlobalOptions Instance = new();
    }
    #endregion
}
