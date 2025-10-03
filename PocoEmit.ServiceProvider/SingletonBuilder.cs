using Microsoft.Extensions.DependencyInjection;
using PocoEmit.Builders;
using PocoEmit.ServiceProvider.Builders;
using System;
using System.Linq.Expressions;

namespace PocoEmit.ServiceProvider;

/// <summary>
/// 单例容器
/// </summary>
/// <param name="provider"></param>
public class SingletonBuilder(ConstantExpression provider)
    : IServiceProviderBuilder
{
    /// <summary>
    /// 单例容器
    /// </summary>
    /// <param name="provider"></param>
    public SingletonBuilder(IServiceProvider provider)
        : this(Expression.Constant(provider, typeof(IKeyedServiceProvider)))
    {
    }
    #region 配置
    private readonly ConstantExpression _provider = provider;
    private readonly ServiceProviderBuilder _builder = new(typeof(IServiceProvider), provider, new());
    /// <summary>
    /// IServiceProvider
    /// </summary>
    public ConstantExpression Provider 
        => _provider;
    /// <summary>
    /// IServiceProvider
    /// </summary>
    public ServiceProviderBuilder Builder 
        => _builder;
    #endregion
    /// <inheritdoc />
    public ServiceProviderBuilder CreateProvider()
        => _builder;
    /// <inheritdoc />
    public ServiceProviderBuilder CreateKeyed()
        => CreateKeyed(_provider, Expression.Parameter(typeof(IKeyedServiceProvider), "provider"));
    /// <summary>
    /// 构造IKeyedServiceProvider
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="keyed"></param>
    /// <returns></returns>
    private static ServiceProviderBuilder CreateKeyed(ConstantExpression provider, ParameterExpression keyed)
        => new(typeof(IKeyedServiceProvider), keyed, new VariableBuilder(keyed, Expression.Convert(provider, typeof(IKeyedServiceProvider))));
}
