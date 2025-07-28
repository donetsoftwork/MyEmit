using PocoEmit.Activators;
using PocoEmit.Builders;
using PocoEmit.Copies;
using PocoEmit.Maping;
using PocoEmit.Reflection;
using System;

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
    /// <param name="reflection"></param>
    public MapperConfigurationBase(IReflectionMember reflection)
        : base(reflection)
    {
        // 初始化配置
        ConvertBuilder = new ComplexConvertBuilder(this);
        _copierFactory = new CopierFactory(this);
        _activatorFactory = new ActivatorFactory(this);
        _primitives = new PrimitiveConfiguration(this);
    }
    #region 配置
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
    /// <summary>
    /// 默认成员匹配
    /// </summary>
    protected IMemberMatch _defaultMatch = MemberNameMatcher.Default;
    #endregion
    #region IMapperOptions
    /// <inheritdoc />
    public IMemberMatch DefaultMatch
    {
        get => _defaultMatch;
        set => _defaultMatch = value ?? throw new ArgumentNullException(nameof(value));
    }
    /// <inheritdoc />
    public CopierFactory CopierFactory
        => _copierFactory;
    /// <inheritdoc />
    public ActivatorFactory ActivatorFactory
        => _activatorFactory;
    /// <inheritdoc />
    public PrimitiveConfiguration Primitives
        => _primitives;
    #endregion
}
