using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Activators;

/// <summary>
/// 带参委托激活
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
/// <param name="activator"></param>
public class ArgumentDelegateActivator<TSource, TDest>(Func<TSource, TDest> activator)
    : MethodActivator(activator.Target, activator.GetMethodInfo()), IEmitActivator
{
    #region 配置
    private readonly Func<TSource, TDest> _activator = activator;
    /// <inheritdoc />
    public Func<TSource, TDest> Activator
        => _activator;
    #endregion
    /// <inheritdoc />
    public override Expression New(Expression argument)
         => Expression.Call(Instance, _method, argument);
}
