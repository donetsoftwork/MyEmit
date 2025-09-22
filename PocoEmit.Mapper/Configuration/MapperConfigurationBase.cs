using PocoEmit.Activators;
using PocoEmit.Builders;
using PocoEmit.Converters;
using PocoEmit.Copies;
using PocoEmit.Maping;
using PocoEmit.Reflection;
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
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
    public MapperConfigurationBase(MapperOptions options)
        : base(options)
    {
        // 初始化配置
        var concurrencyLevel = options.ConcurrencyLevel;
        _copiers = new ConcurrentDictionary<PairTypeKey, IEmitCopier>(concurrencyLevel, options.CopierCapacity);
        _copyConfiguration = new ConcurrentDictionary<PairTypeKey, IEmitCopier>(concurrencyLevel, options.CopierConfigurationCapacity);
        _activeConfiguration = new ConcurrentDictionary<Type, IEmitActivator>(concurrencyLevel, options.ActivatorConfigurationCapacity);
        _argumentActiveConfiguration = new ConcurrentDictionary<PairTypeKey, IEmitActivator>(concurrencyLevel, options.ArgumentActivatorConfigurationCapacity);
        _checkMembers = new ConcurrentDictionary<PairTypeKey, Delegate>();
        _matchConfiguration = new ConcurrentDictionary<PairTypeKey, IMemberMatch>(concurrencyLevel, options.MatchCapacity);
        _primitiveTypes = new ConcurrentDictionary<Type, bool>(concurrencyLevel, options.PrimitiveCapacity);
        _defaultValueConfiguration = new ConcurrentDictionary<Type, IBuilder<Expression>>(concurrencyLevel, options.DefaultValueCapacity);
        _contextConverters = new ConcurrentDictionary<PairTypeKey, IContextConverter>(concurrencyLevel, options.ContextConverterCapacity);
        _reflectionConstructor = DefaultReflectionConstructor.Default;
        _defaultMatcher = MemberNameMatcher.Default;
        _recognizer = new Recognizer(_defaultMatcher.NameMatch);
        ConvertBuilder = new ComplexConvertBuilder(this);
        _copierBuilder = new CopierBuilder(this);
        _copierFactory = new CopierFactory(this);
        _primitives = new PrimitiveConfiguration(this);
    }
    #region 配置
    private CopierBuilder _copierBuilder;
    private IReflectionConstructor _reflectionConstructor;
    /// <summary>
    /// 默认成员匹配
    /// </summary>
    protected IMemberMatch _defaultMatcher;
    /// <summary>
    /// 识别器
    /// </summary>
    private readonly IRecognizer _recognizer;
    /// <summary>
    /// 复制器
    /// </summary>
    private readonly CopierFactory _copierFactory;
    /// <summary>
    /// 基础类型配置
    /// </summary>
    private readonly PrimitiveConfiguration _primitives;
    /// <summary>
    /// 反射获取构造函数
    /// </summary>
    public IReflectionConstructor ReflectionConstructor
    {
        get => _reflectionConstructor;
        internal set => _reflectionConstructor = value;
    }
    #endregion
    #region IMapperOptions
    /// <inheritdoc />
    public IMemberMatch DefaultMatcher
    {
        get => _defaultMatcher;
        set => _defaultMatcher = value ?? throw new ArgumentNullException(nameof(value));
    }
    /// <inheritdoc />
    public IRecognizer Recognizer 
        => _recognizer;
    /// <inheritdoc />
    public CopierBuilder CopierBuilder 
    { 
        get => _copierBuilder; 
        internal set => _copierBuilder = value ?? throw new ArgumentNullException(nameof(value));
    }
    #endregion
    #region 功能
    /// <summary>
    /// 获取Emit类型复制器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual IEmitCopier GetEmitCopier(PairTypeKey key)
        => _copierFactory.Get(key);
    /// <summary>
    /// 是否基础类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool CheckPrimitive(Type type)
        => _primitives.Get(type);
    /// <inheritdoc />
    public virtual ConstructorInfo GetConstructor(Type instanceType)
        => _reflectionConstructor.GetConstructor(instanceType);
    /// <inheritdoc />
    public IEmitActivator GetEmitActivator(PairTypeKey key)
    {
        if (_argumentActiveConfiguration.TryGetValue(key, out IEmitActivator activator) || _activeConfiguration.TryGetValue(key.RightType, out activator))
            return activator;
        return activator;
    }
    /// <summary>
    /// 获取默认值构建器
    /// </summary>
    /// <param name="destType"></param>
    /// <returns></returns>
    public IBuilder<Expression> GetDefaultValueBuilder(Type destType)
    {
        _defaultValueConfiguration.TryGetValue(destType, out IBuilder<Expression> builder);
        return builder;
    }
    /// <inheritdoc />
    public Expression CreateDefault(Type destType)
    {
        if (_defaultValueConfiguration.TryGetValue(destType, out IBuilder<Expression> builder))
            return builder.Build();
        return null;
    }
    #endregion
}
