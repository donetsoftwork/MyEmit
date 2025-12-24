using PocoEmit.Builders;
using PocoEmit.Complexes;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Activators;

/// <summary>
/// 委托激活类型
/// </summary>
/// <typeparam name="TInstance"></typeparam>
/// <param name="activator"></param>
public class DelegateActivator<TInstance>(Expression<Func<TInstance>> activator)
    : IEmitActivator
{
    #region 配置
    private readonly Expression<Func<TInstance>> _activator = activator;
    /// <inheritdoc />
    public Expression<Func<TInstance>> Activator
        => _activator;
    /// <inheritdoc />
    public Type ReturnType
        => typeof(TInstance);
    #endregion
    /// <inheritdoc />
    Expression IEmitActivator.New(IBuildContext context, IEmitBuilder builder, Expression argument)
        => _activator.Body;
}
