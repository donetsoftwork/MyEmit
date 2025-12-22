using Microsoft.Extensions.DependencyInjection;
using PocoEmit.Builders;
using PocoEmit.ServiceProvider.Builders;
using System;
using System.Linq.Expressions;

namespace PocoEmit.ServiceProvider;

/// <summary>
/// 容器作用域服务构造器
/// </summary>
/// <param name="root"></param>
public class ScopeServiceBuilder(ConstantExpression root)
    : IServiceProviderBuilder
{
    /// <summary>
    /// 容器包装
    /// </summary>
    /// <param name="root"></param>
    public ScopeServiceBuilder(IServiceProvider root)
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
    public virtual ProviderBuilder CreateProvider()
        => CreateProvider(_root, Expression.Parameter(typeof(IServiceProvider), "provider"));
    /// <inheritdoc />
    public virtual ProviderBuilder CreateKeyed()
        => CreateKeyed(_root, Expression.Parameter(typeof(IKeyedServiceProvider), "provider"));
    /// <summary>
    /// 构造IServiceProvider
    /// </summary>
    /// <param name="root"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    private static ProviderBuilder CreateProvider(ConstantExpression root, ParameterExpression provider)
    {
        var builder = new VariableBuilder(provider, EmitProviderHelper.CallGetProvider(root));
        return new(typeof(IServiceProvider), provider, builder);
    }
    /// <summary>
    /// 构造IKeyedServiceProvider
    /// </summary>
    /// <param name="root"></param>
    /// <param name="keyed"></param>
    /// <returns></returns>
    private static ProviderBuilder CreateKeyed(ConstantExpression root, ParameterExpression keyed)
    {
        var builder = new VariableBuilder(keyed, EmitProviderHelper.CallGetKeyedProvider(root));
        return new(typeof(IKeyedServiceProvider), keyed, builder);
    }
}
