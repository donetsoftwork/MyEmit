using PocoEmit.Builders;
using PocoEmit.Collections;
using PocoEmit.Converters;
using PocoEmit.Reflection;
using PocoEmit.Visitors;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Configuration;

/// <summary>
/// Emit配置
/// </summary>
public abstract partial class ConfigurationBase 
    : IPocoOptions
{
    /// <summary>
    /// Emit配置
    /// </summary>
    /// <param name="options"></param>
    protected ConfigurationBase(PocoOptions options)
    {
        // 初始化配置
        var concurrencyLevel = options.ConcurrencyLevel;
        _converters = new ConcurrentDictionary<PairTypeKey, IEmitConverter>(concurrencyLevel, options.ConverterCapacity);
        _convertConfiguration = new ConcurrentDictionary<PairTypeKey, IEmitConverter>(concurrencyLevel, options.ConverterConfigurationCapacity);
        _memberBundles = new ConcurrentDictionary<Type, MemberBundle>(concurrencyLevel, options.MemberBundleCapacity);
        _reflectionMember = DefaultReflectionMember.Default;
        _convertBuilder = new ConvertBuilder(this);
        _converterFactory = new(this);
        _memberCacher = new TypeMemberCacher(this);
        _lambdaInvoke = options.LambdaInvoke;
    }
    #region 配置
    /// <summary>
    /// 转换器工厂
    /// </summary>
    private readonly ConverterFactory _converterFactory;
    /// <summary>
    /// 构建转换器
    /// </summary>
    private ConvertBuilder _convertBuilder;
    /// <summary>
    /// 反射获取成员
    /// </summary>
    private IReflectionMember _reflectionMember;
    /// <summary>
    /// 成员缓存器
    /// </summary>
    private readonly TypeMemberCacher _memberCacher;
    /// <summary>
    /// 读成员缓存
    /// </summary>
    private ReadFuncCacher _readerFuncCacher = null;
    /// <summary>
    /// 写成员缓存
    /// </summary>
    private WriteActionCacher _writerActionCacher = null;
    private bool _lambdaInvoke = true;
    /// <inheritdoc />
    public IEnumerable<IEmitConverter> Converters
        => _converters.Values;
    /// <summary>
    /// 反射获取成员
    /// </summary>
    public IReflectionMember ReflectionMember
    {
        get => _reflectionMember;
        internal set => _reflectionMember = value;
    }
    /// <inheritdoc />
    public ConvertBuilder ConvertBuilder
    {
        get => _convertBuilder;
        internal set => _convertBuilder = value;
    }
    /// <summary>
    /// 成员缓存器
    /// </summary>
    public TypeMemberCacher MemberCacher
        => _memberCacher;
    /// <summary>
    /// 是Invoke,还是内嵌表达式
    /// </summary>
    public bool LambdaInvoke
        => _lambdaInvoke;
    #endregion
    #region 功能
    /// <inheritdoc />
    public Func<object, object> GetReadFunc(MemberInfo member)
        => (_readerFuncCacher ??= new ReadFuncCacher(this)).Get(member);
    /// <inheritdoc />
    public Action<object, object> GetWriteAction(MemberInfo member)
        => (_writerActionCacher ??= new WriteActionCacher(this)).Get(member);
    /// <inheritdoc />
    public IEmitConverter GetEmitConverter(in PairTypeKey key)
        => _converterFactory.Get(key);
    /// <summary>
    /// 调用
    /// </summary>
    /// <param name="lambda"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    public Expression Call(LambdaExpression lambda, params Expression[] arguments)
    {
        if (_lambdaInvoke) 
            return Expression.Invoke(lambda, arguments);
        var body = lambda.Body;
        // 通过参数替换调用
        var replaceVisitor = ReplaceVisitor.Create(true, lambda.Parameters, arguments);
        if (replaceVisitor is null)
            return body;
        return replaceVisitor.Visit(body);
    }
    #endregion
}
