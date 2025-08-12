using PocoEmit.Builders;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Activators;

/// <summary>
/// 委托激活类型
/// </summary>
/// <typeparam name="TInstance"></typeparam>
/// <param name="activator"></param>
public class DelegateActivator<TInstance>(Func<TInstance> activator)
    : MethodActivator(EmitHelper.CheckMethodCallInstance(activator), activator.GetMethodInfo()), IGenericActivator<TInstance>
{
    #region 配置
    private readonly Func<TInstance> _activator = activator;
    /// <inheritdoc />
    public Func<TInstance> Activator
        => _activator;
    #endregion
    /// <inheritdoc />
    public override Expression New(Expression argument)
        => Expression.Call(_target, _method);
}
