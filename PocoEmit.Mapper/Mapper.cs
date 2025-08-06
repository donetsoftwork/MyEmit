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
/// 对象映射配置
/// </summary>
public sealed class Mapper : MapperConfigurationBase
{
    /// <summary>
    /// 对象映射配置
    /// </summary>
    /// <param name="reflectionMember"></param>
    /// <param name="reflectionConstructor"></param>
    /// <param name="defaultMatch"></param>
    private Mapper(IReflectionMember reflectionMember, IReflectionConstructor reflectionConstructor, IMemberMatch defaultMatch)
        : base(reflectionMember, reflectionConstructor, defaultMatch, new InheritRecognizer(GlobalOptions.Instance.Recognizer)) 
    { 
    }
    #region IMapperOptions
    #region 功能
    #region IEmitOptions
    /// <inheritdoc />
    public override Func<object, object> GetReadFunc(MemberInfo member)
        => base.GetReadFunc(member) ?? Global.GetReadFunc(member);
    /// <inheritdoc />
    public override Action<object, object> GetWriteAction(MemberInfo member)
        => base.GetWriteAction(member) ?? Global.GetWriteAction(member);
    /// <inheritdoc />
    public override IEmitConverter GetEmitConverter(MapTypeKey key)
        => base.GetEmitConverter(key) ?? Global.GetEmitConverter(key);
    #endregion
    /// <summary>
    /// 获取Emit类型复制器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public override IEmitCopier GetEmitCopier(MapTypeKey key)
        => base.GetEmitCopier(key) ?? GlobalOptions.Instance.GetEmitCopier(key);
    #endregion
    #region 配置
    /// <inheritdoc />
    internal override bool TryRead(MapTypeKey key, out IEmitConverter value)
        => base.TryRead(key, out value) || GlobalOptions.Instance.TryRead(key, out value);
    /// <inheritdoc />
    internal override bool TryRead(MapTypeKey key, out IEmitCopier value)
        => base.TryRead(key, out value) || GlobalOptions.Instance.TryRead(key, out value);
    /// <inheritdoc />
    internal override bool TryRead(Type key, out IEmitActivator value)
        => base.TryRead(key, out value) || GlobalOptions.Instance.TryRead(key, out value);
    /// <inheritdoc />
    internal override bool TryRead(MapTypeKey key, out IEmitActivator value)
        => base.TryRead(key, out value) || GlobalOptions.Instance.TryRead(key, out value);
    /// <inheritdoc />
    internal override bool TryRead(MapTypeKey key, out IMemberMatch value)
        => base.TryRead(key, out value) || GlobalOptions.Instance.TryRead(key, out value);
    /// <inheritdoc />
    internal override bool TryRead(Type key, out object value)
        => base.TryRead(key, out value) || GlobalOptions.Instance.TryRead(key, out value);
    /// <inheritdoc />
    public override bool TryGetValue(Type key, out bool value)
        => base.TryGetValue(key, out value) || GlobalOptions.Instance.TryGetValue(key, out value);
    #endregion
    #endregion
    #region Create
    /// <summary>
    /// 构造映射器
    /// </summary>
    /// <param name="reflectionMember"></param>
    /// <param name="reflectionConstructor"></param>
    /// <param name="defaultMatch"></param>
    /// <returns></returns>
    public static IMapper Create(IReflectionMember reflectionMember, IReflectionConstructor reflectionConstructor, IMemberMatch defaultMatch)
        => new Mapper(reflectionMember, reflectionConstructor, defaultMatch);
    /// <summary>
    /// 构造映射器
    /// </summary>
    /// <param name="reflectionMember"></param>
    /// <param name="reflectionConstructor"></param>
    /// <returns></returns>
    public static IMapper Create(IReflectionMember reflectionMember, IReflectionConstructor reflectionConstructor)
        => new Mapper(reflectionMember, reflectionConstructor, GlobalOptions.Instance.DefaultMatcher);
    /// <summary>
    /// 构造映射器
    /// </summary>
    /// <returns></returns>
    public static IMapper Create()
        => new Mapper(DefaultReflectionMember, DefaultReflectConstructor, GlobalOptions.Instance.DefaultMatcher);
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
    public static IMapper Global
        => GlobalOptions.Instance;
    /// <summary>
    /// 全局配置
    /// </summary>
    sealed class GlobalOptions()
        : MapperConfigurationBase(DefaultReflectionMember, DefaultReflectConstructor, MemberNameMatcher.Default, new Recognizer(MemberNameMatcher.Default.NameMatch))
    {
        /// <summary>
        /// Emit全局配置
        /// </summary>
        public static readonly GlobalOptions Instance = new();
    }
    #endregion
}
