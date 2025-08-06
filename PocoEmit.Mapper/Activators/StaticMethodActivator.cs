using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Activators;

/// <summary>
/// 方法激活类型
/// </summary>
/// <param name="target"></param>
/// <param name="method"></param>
public class MethodActivator(object target, MethodInfo method)
    : IEmitActivator
{
    #region 配置
    private readonly object _target = target;
    /// <summary>
    /// 实例
    /// </summary>
    public object Target
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
    /// <summary>
    /// 调用实例
    /// </summary>
    public Expression Instance
        => ReflectionHelper.CheckMethodCallInstance(_target);
    #endregion
    /// <inheritdoc />
    public virtual Expression New(Expression argument)
        => Expression.Call(Instance, _method);
}
