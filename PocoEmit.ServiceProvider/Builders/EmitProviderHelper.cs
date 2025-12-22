using Hand.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PocoEmit.Builders;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.ServiceProvider.Builders;

/// <summary>
/// Emit工具
/// </summary>
public static class EmitProviderHelper
{
    #region BuildService
    /// <summary>
    /// 调用GetService
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="provider"></param>
    /// <param name="serviceType"></param>
    public static Expression CallGetService(EmitBuilder builder, Expression provider, Type serviceType)
        => ConvertGuard(builder, Expression.Call(provider, GetServiceMethod, Expression.Constant(serviceType)), serviceType);
    /// <summary>
    /// 调用GetKeyedService
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="provider"></param>
    /// <param name="serviceType"></param>
    /// <param name="serviceKey"></param>
    public static Expression CallGetKeydService(EmitBuilder builder, Expression provider, Type serviceType, Expression serviceKey)
         => ConvertGuard(builder, Expression.Call(provider, GetKeydServiceMethod, Expression.Constant(serviceType), serviceKey), serviceType);
    /// <summary>
    /// 安全转换
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="source"></param>
    /// <param name="serviceType"></param>
    private static Expression ConvertGuard(EmitBuilder builder, Expression source, Type serviceType)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var isValueType = serviceType.GetTypeInfo().IsValueType;
#else
        var isValueType = serviceType.IsValueType;
#endif
        if (isValueType && !ReflectionType.IsNullable(serviceType))
        {
            // 值类型且非Nullable,直接转换会报错
            var temp = builder.Temp<object>(source);
            //builder.IfDefault(temp, Expression.Default(serviceType), Expression.Convert(temp, serviceType), serviceType);
            return EmitHelper.IfDefault(temp, Expression.Default(serviceType), Expression.Convert(temp, serviceType), serviceType);
        }
        else
        {            
            return Expression.Convert(source, serviceType);
        }
    }
    /// <summary>
    /// 调用GetProvider
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    public static Expression CallGetProvider(Expression root)
        => Expression.Call(null, GetProviderMethod, root);
    /// <summary>
    /// 调用GetKeyedProvider
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    public static Expression CallGetKeyedProvider(Expression root)
        => Expression.Call(null, GetKeyedProviderMethod, root);
    #endregion
    #region Reflection
    /// <summary>
    /// 反射GetService方法
    /// </summary>
    internal static readonly MethodInfo GetServiceMethod = EmitHelper.GetMethodInfo(() => ((IServiceProvider)null).GetService(typeof(int)));
    /// <summary>
    /// 反射GetKeyedService方法
    /// </summary>
    internal static readonly MethodInfo GetKeydServiceMethod = EmitHelper.GetMethodInfo(() => ((IKeyedServiceProvider)null).GetKeyedService(typeof(int), null));
    /// <summary>
    /// 反射GetProvider方法
    /// </summary>
    internal static readonly MethodInfo GetProviderMethod = EmitHelper.GetMethodInfo(() => GetProvider(null));
    /// <summary>
    /// 反射GetKeyedProvider方法
    /// </summary>
    internal static readonly MethodInfo GetKeyedProviderMethod = EmitHelper.GetMethodInfo(() => GetKeyedProvider(null));
    #endregion
    /// <summary>
    /// 获取服务提供器
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    private static IServiceProvider GetProvider(IServiceProvider root)
        => root.GetService<IServiceProvider>() ?? root;
    /// <summary>
    /// 获取含键服务提供器
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    private static IKeyedServiceProvider GetKeyedProvider(IServiceProvider root)
        => root.GetService<IKeyedServiceProvider>() ?? (IKeyedServiceProvider)root;
}
