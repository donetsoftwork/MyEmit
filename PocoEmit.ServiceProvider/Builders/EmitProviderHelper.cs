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
    /// <param name="provider"></param>
    /// <param name="serviceType"></param>
    /// <returns></returns>
    public static Expression CallGetService(Expression provider, Type serviceType)
        => Expression.Convert(Expression.Call(provider, GetServiceMethod, Expression.Constant(serviceType)), serviceType);
    /// <summary>
    /// 调用GetKeyedService
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="serviceType"></param>
    /// <param name="serviceKey"></param>
    /// <returns></returns>
    public static Expression CallGetKeydService(Expression provider, Type serviceType, Expression serviceKey)
        => Expression.Convert(Expression.Call(provider, GetKeydServiceMethod, Expression.Constant(serviceType), serviceKey), serviceType);
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
    #endregion
}
