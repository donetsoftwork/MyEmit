using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Activators;

/// <summary>
/// 方法激活类型
/// </summary>
/// <param name="target"></param>
/// <param name="method"></param>
public class MethodActivator(Expression target, MethodInfo method)
    : IEmitActivator
{
    #region 配置
    /// <summary>
    /// 实例
    /// </summary>
    protected readonly Expression _target = target;
    /// <summary>
    /// 实例
    /// </summary>
    public Expression Target
        => _target;
    /// <summary>
    /// 方法
    /// </summary>
    protected readonly MethodInfo _method = method;
    /// <inheritdoc />
    public Type ReturnType 
        => _method.ReturnType;
    /// <summary>
    /// 方法
    /// </summary>
    public MethodInfo Method 
        => _method;
    #endregion
    /// <inheritdoc />
    public virtual Expression New(Expression argument)
        => Expression.Call(_target, _method);
}
