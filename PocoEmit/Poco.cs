using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Reflection;
using System;

namespace PocoEmit;

/// <summary>
/// 简单对象配置
/// </summary>
/// <param name="reflection"></param>
public sealed class Poco(IReflectionMember reflection)
    : ConfigurationBase(reflection)
{
    /// <summary>
    /// 简单对象配置
    /// </summary>
    public Poco()
        : this(DefaultReflect)
    {
    }
    #region ISettings<MapTypeKey, IEmitTypeConverter>
    /// <inheritdoc />
    public override bool TryGetValue(MapTypeKey key, out IEmitConverter value)
    {
        return base.TryGetValue(key, out value) 
            || Global.TryGetValue(key, out value);
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
    public static IPocoOptions Global
        => GlobalOptions.Instance;
    /// <summary>
    /// Emit全局配置
    /// </summary>
    sealed class GlobalOptions()
        : ConfigurationBase(DefaultReflect)
    {
        /// <summary>
        /// Emit全局配置
        /// </summary>
        public static readonly GlobalOptions Instance = new();
    }
    #endregion
}