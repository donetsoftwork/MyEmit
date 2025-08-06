using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Reflection;
using System;
using System.Reflection;

namespace PocoEmit;

/// <summary>
/// 简单对象配置
/// </summary>
public sealed class Poco
    : ConfigurationBase
{
    /// <summary>
    /// 简单对象配置
    /// </summary>
    /// <param name="reflection"></param>
    private Poco(IReflectionMember reflection)
        : base(reflection)
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
    /// <summary>
    /// 简单对象处理对象
    /// </summary>
    /// <param name="reflection"></param>
    /// <returns></returns>
    public static IPoco Create(IReflectionMember reflection)
        => new Poco(reflection);
    /// <summary>
    /// 简单对象处理对象
    /// </summary>
    /// <returns></returns>
    public static IPoco Create()
        => new Poco(DefaultReflectMember);
    #endregion
    #region 配置
    /// <inheritdoc />
    internal override bool TryRead(MapTypeKey key, out IEmitConverter value)
        => base.TryRead(key, out value) || GlobalOptions.Instance.TryRead(key, out value);
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
    public static IPoco Global
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