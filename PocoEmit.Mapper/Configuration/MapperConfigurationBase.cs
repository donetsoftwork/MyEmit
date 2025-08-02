using PocoEmit.Activators;
using PocoEmit.Builders;
using PocoEmit.Copies;
using PocoEmit.Maping;
using PocoEmit.Reflection;
using System;
using System.Reflection;

namespace PocoEmit.Configuration;

/// <summary>
/// 映射配置基类
/// </summary>
public abstract partial class MapperConfigurationBase
    : ConfigurationBase
    , IMapperOptions
{
    /// <summary>
    /// 映射配置基类
    /// </summary>
    /// <param name="reflectionMember"></param>
    /// <param name="reflectionConstructor"></param>
    /// <param name="defaultMatch"></param>
    public MapperConfigurationBase(IReflectionMember reflectionMember, IReflectionConstructor reflectionConstructor, IMemberMatch defaultMatch)
        : base(reflectionMember)
    {
        // 初始化配置
        _reflectionConstructor = reflectionConstructor;
        _defaultMatch = defaultMatch;
        ConvertBuilder = new ComplexConvertBuilder(this);
        _copierFactory = new CopierFactory(this);
        _activatorFactory = new ActivatorFactory(this);
        _primitives = new PrimitiveConfiguration(this);
    }
    #region 配置
    private IReflectionConstructor _reflectionConstructor;
    /// <summary>
    /// 默认成员匹配
    /// </summary>
    protected IMemberMatch _defaultMatch;
    /// <summary>
    /// 复制器
    /// </summary>
    private readonly CopierFactory _copierFactory;
    /// <summary>
    /// 激活器
    /// </summary>
    private readonly ActivatorFactory _activatorFactory;
    /// <summary>
    /// 基础类型配置
    /// </summary>
    private readonly PrimitiveConfiguration _primitives;
    #endregion
    /// <summary>
    /// 反射获取构造函数
    /// </summary>
    public IReflectionConstructor ReflectionConstructor
    {
        get => _reflectionConstructor;
        internal set => _reflectionConstructor = value;
    }
    #region IMapperOptions
    /// <inheritdoc />
    public IMemberMatch DefaultMatch
    {
        get => _defaultMatch;
        set => _defaultMatch = value ?? throw new ArgumentNullException(nameof(value));
    }
    #endregion
    #region 功能
    /// <summary>
    /// 获取Emit类型复制器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual IEmitCopier GetEmitCopier(MapTypeKey key)
        => _copierFactory.Get(key);
    /// <summary>
    /// 获取Emit类型激活器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual IEmitActivator GetEmitActivatorr(Type key)
        => _activatorFactory.Get(key);
    /// <summary>
    /// 是否基础类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool CheckPrimitive(Type type)
        => _primitives.Get(type);
    /// <inheritdoc />
    public ConstructorInfo GetConstructor(Type instanceType)
        => _reflectionConstructor.GetConstructor(instanceType);
    #endregion
}
