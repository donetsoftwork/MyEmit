using PocoEmit.Activators;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Copies;
using PocoEmit.Maping;
using PocoEmit.Reflection;
using System;
using System.Reflection;

namespace PocoEmit;

/// <summary>
/// 映射配置
/// </summary>
/// <param name="reflectionMember"></param>
/// <param name="reflectionConstructor"></param>
/// <param name="defaultMatch"></param>
public sealed class Mapper(IReflectionMember reflectionMember, IReflectionConstructor reflectionConstructor, IMemberMatch defaultMatch)
    : MapperConfigurationBase(reflectionMember, reflectionConstructor, defaultMatch)
{
    /// <summary>
    /// Emit配置
    /// </summary>
    public Mapper()
        : this(DefaultReflectionMember, DefaultReflectConstructor, GlobalOptions.Instance.DefaultMatch)
    {
    }
    #region IMapperOptions
    #region IEmitOptions
    #region 功能
    /// <inheritdoc />
    public override Func<object, object> GetReadFunc(MemberInfo member)
        => base.GetReadFunc(member) ?? Global.GetReadFunc(member);
    /// <inheritdoc />
    public override Action<object, object> GetWriteAction(MemberInfo member)
        => base.GetWriteAction(member) ?? Global.GetWriteAction(member);
    /// <inheritdoc />
    public override IEmitConverter GetEmitConverter(MapTypeKey key)
        => base.GetEmitConverter(key) ?? Global.GetEmitConverter(key);
    /// <inheritdoc />
    public override bool TryGetConvertSetting(MapTypeKey key, out IEmitConverter value)
        => base.TryGetConvertSetting(key, out value) || GlobalOptions.Instance.TryGetConvertSetting(key, out value);
    #endregion
    #endregion
    #region 功能
    /// <summary>
    /// 获取Emit类型复制器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public override IEmitCopier GetEmitCopier(MapTypeKey key)
        => base.GetEmitCopier(key) ?? GlobalOptions.Instance.GetEmitCopier(key);
    /// <summary>
    /// 获取Emit类型激活器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public override IEmitActivator GetEmitActivatorr(Type key)
        => base.GetEmitActivatorr(key) ?? GlobalOptions.Instance.GetEmitActivatorr(key);
    #endregion
    /// <inheritdoc />
    public override IMemberMatch GetMemberMatch(MapTypeKey key)
    {
        if (_matches.TryGetValue(key, out IMemberMatch match) || GlobalOptions.Instance._matches.TryGetValue(key, out match))
            return match;
        return _defaultMatch;
    }
    /// <inheritdoc />
    public override bool TryGetValue(MapTypeKey key, out IMemberMatch value)
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
    private static IReflectionMember _defaultReflectionMember = Reflection.DefaultReflectionMember.Default;
    /// <summary>
    /// 默认反射成员对象
    /// </summary>
    public static IReflectionMember DefaultReflectionMember
    {
        get => _defaultReflectionMember;
        set
        {
            _defaultReflectionMember = value ?? throw new ArgumentNullException(nameof(value));
            GlobalOptions.Instance.ReflectionMember = value;
        }
    }
    private static IReflectionConstructor _defaultReflectConstructor = DefaultReflectionConstructor.Default;
    /// <summary>
    /// 默认反射构造函数
    /// </summary>
    public static IReflectionConstructor DefaultReflectConstructor
    {
        get => _defaultReflectConstructor;
        set
        {
            _defaultReflectConstructor = value ?? throw new ArgumentNullException(nameof(value));
            GlobalOptions.Instance.ReflectionConstructor = value;
        }
    }
    /// <summary>
    /// 全局配置
    /// </summary>
    public static IMapperOptions Global
        => GlobalOptions.Instance;
    /// <summary>
    /// 全局配置
    /// </summary>
    sealed class GlobalOptions()
        : MapperConfigurationBase(DefaultReflectionMember, DefaultReflectConstructor, MemberNameMatcher.Default)
    {
        /// <summary>
        /// Emit全局配置
        /// </summary>
        public static readonly GlobalOptions Instance = new();
    }
    #endregion
}
