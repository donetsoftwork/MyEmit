using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PocoEmit.Builders;
using PocoEmit.ServiceProvider;
using PocoEmit.ServiceProvider.Builders;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Mvc;

/// <summary>
/// 容器上下文构造器
/// </summary>
/// <param name="root"></param>
/// <param name="accessor"></param>
public class ContextBuilder(IServiceProvider root, ConstantExpression accessor)
    : ScopeServiceBuilder(root)
{
    /// <summary>
    /// 上下文容器
    /// </summary>
    /// <param name="root"></param>
    /// <param name="accessor"></param>
    public ContextBuilder(IServiceProvider root, IHttpContextAccessor accessor)
        : this(root, Expression.Constant(accessor, typeof(IHttpContextAccessor)))
    {
    }
    #region 配置
    private readonly ConstantExpression _accessor = accessor;
    /// <summary>
    /// http上下文读取器
    /// </summary>
    public ConstantExpression Accessor 
        => _accessor;
    #endregion
    /// <inheritdoc />
    public override ProviderBuilder CreateProvider()
        => CreateProvider(_root, _accessor, Expression.Parameter(typeof(IServiceProvider), "provider"));
    /// <inheritdoc />
    public override ProviderBuilder CreateKeyed()
        => CreateKeyed(_root, _accessor, Expression.Parameter(typeof(IKeyedServiceProvider), "provider"));
    ///// <inheritdoc />
    //public override IServiceProvider GetServiceProvider()
    //{
    //    var context = _accessor.HttpContext;
    //    if (context is null)
    //        return base.GetServiceProvider();
    //    return context.RequestServices;
    //}
    ///// <inheritdoc />
    //public override IKeyedServiceProvider GetKeyedServiceProvider()
    //{
    //    var context = _accessor.HttpContext;
    //    if (context is null)
    //        return base.GetKeyedServiceProvider();
    //    return context.RequestServices as IKeyedServiceProvider ?? base.GetKeyedServiceProvider();
    //}
    /// <summary>
    /// 构造IServiceProvider
    /// </summary>
    /// <param name="root"></param>
    /// <param name="accessor"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    private static ProviderBuilder CreateProvider(ConstantExpression root, ConstantExpression accessor, ParameterExpression provider)
    {
        var context = Expression.Parameter(typeof(HttpContext), "context");
        // var context = accessor.HttpContext
        var builder = new VariableBuilder(context, Expression.Property(accessor, _httpContextProperty));
        builder.Change(provider);
        // if(context is null)
        //  provider = GetProvider(root);
        // else
        //  provider = context.RequestServices;
        builder.AssignIfDefault(context, EmitProviderHelper.CallGetProvider(root), Expression.Property(context, _requestServicesProperty));        
        return new(typeof(IServiceProvider), provider, builder);
    }
    /// <summary>
    /// 构造IKeyedServiceProvider
    /// </summary>
    /// <param name="root"></param>
    /// <param name="accessor"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    private static ProviderBuilder CreateKeyed(ConstantExpression root, ConstantExpression accessor, ParameterExpression provider)
    {
        var context = Expression.Parameter(typeof(HttpContext), "context");
        // var context = accessor.HttpContext
        var builder = new VariableBuilder(context, Expression.Property(accessor, _httpContextProperty));
        builder.Change(provider);
        // if(context is null)
        //  provider = GetKeyedProvider(root);
        // else
        //  provider = (IKeyedServiceProvider)context.RequestServices;
        builder.AssignIfDefault(context, EmitProviderHelper.CallGetKeyedProvider(root), Expression.Convert(Expression.Property(context, _requestServicesProperty), typeof(IKeyedServiceProvider)));
        return new(typeof(IKeyedServiceProvider), provider, builder);
    }
    /// <summary>
    /// 反射HttpContext属性
    /// </summary>
    private static readonly PropertyInfo _httpContextProperty = EmitHelper.GetPropertyInfo<IHttpContextAccessor, HttpContext>(accessor => accessor.HttpContext);
    /// <summary>
    /// 反射RequestServices属性
    /// </summary>
    private static readonly PropertyInfo _requestServicesProperty = EmitHelper.GetPropertyInfo<HttpContext, IServiceProvider>(context => context.RequestServices);
}
