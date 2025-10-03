using Microsoft.Extensions.DependencyInjection;
using PocoEmit.Builders;
using PocoEmit.ServiceProvider.Builders;
using System;
using System.Linq.Expressions;

namespace PocoEmit.ServiceProvider;

/// <summary>
/// 容器包装
/// </summary>
/// <param name="root"></param>
public class ScopeBuilder(ConstantExpression root)
    : IServiceProviderBuilder
{
    /// <summary>
    /// 容器包装
    /// </summary>
    /// <param name="root"></param>
    public ScopeBuilder(IServiceProvider root)
        : this(Expression.Constant(root, typeof(IServiceProvider))) 
    {
    }
    #region 配置
    /// <summary>
    /// 根容器
    /// </summary>
    protected readonly ConstantExpression _root = root;
    /// <summary>
    /// 根容器
    /// </summary>
    public ConstantExpression Root 
        => _root;
    #endregion
    /// <inheritdoc />
    public virtual ServiceProviderBuilder CreateProvider()
        => CreateProvider(_root, Expression.Parameter(typeof(IServiceProvider), "provider"));
    /// <inheritdoc />
    public virtual ServiceProviderBuilder CreateKeyed()
        => CreateKeyed(_root, Expression.Parameter(typeof(IKeyedServiceProvider), "provider"));
    /// <summary>
    /// 构造IServiceProvider
    /// </summary>
    /// <param name="root"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    private static ServiceProviderBuilder CreateProvider(ConstantExpression root, ParameterExpression provider)
    {
        var builder = new VariableBuilder(provider, EmitProviderHelper.CallGetService(root, typeof(IServiceProvider)));
        builder.AssignIfNull(root);
        return new(typeof(IServiceProvider), provider, builder);
    }
    /// <summary>
    /// 构造IKeyedServiceProvider
    /// </summary>
    /// <param name="root"></param>
    /// <param name="keyed"></param>
    /// <returns></returns>
    private static ServiceProviderBuilder CreateKeyed(ConstantExpression root, ParameterExpression keyed)
    {
        var builder = new VariableBuilder(keyed, EmitProviderHelper.CallGetService(root, typeof(IKeyedServiceProvider)));
        builder.AssignIfNull(Expression.Convert(root, typeof(IKeyedServiceProvider)));
        return new(typeof(IKeyedServiceProvider), keyed, builder);
    }
}
