using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Reflection;
using System;
using System.Reflection;

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
        : this(DefaultReflectMember)
    {
    }
    #region 功能
    /// <inheritdoc />
    public override Func<object, object> GetReadFunc(MemberInfo member)
        => base.GetReadFunc(member) ?? GlobalOptions.Instance.GetReadFunc(member);
    /// <inheritdoc />
    public override Action<object, object> GetWriteAction(MemberInfo member)
        => base.GetWriteAction(member) ?? GlobalOptions.Instance.GetWriteAction(member);
    /// <inheritdoc />
    public override IEmitConverter GetEmitConverter(MapTypeKey key)
        => base.GetEmitConverter(key) ?? GlobalOptions.Instance.GetEmitConverter(key);
    /// <inheritdoc />
    public override bool TryGetConvertSetting(MapTypeKey key, out IEmitConverter value)
        => base.TryGetConvertSetting(key, out value) || GlobalOptions.Instance.TryGetConvertSetting(key, out value);
    #endregion
    #region Global
    private static IReflectionMember _defaultReflectMember = DefaultReflectionMember.Default;
    /// <summary>
    /// 默认反射成员对象
    /// </summary>
    public static IReflectionMember DefaultReflectMember
    {
        get => _defaultReflectMember;
        set
        {
            _defaultReflectMember = value ?? throw new ArgumentNullException(nameof(value));
            GlobalOptions.Instance.ReflectionMember = value;
        }
    }
    /// <summary>
    /// 全局配置
    /// </summary>
    public static IPocoOptions Global
        => GlobalOptions.Instance;
    /// <summary>
    /// Emit全局配置
    /// </summary>
    sealed class GlobalOptions()
        : ConfigurationBase(DefaultReflectMember)
    {
        /// <summary>
        /// Emit全局配置
        /// </summary>
        public static readonly GlobalOptions Instance = new();
    }
    #endregion
}