using PocoEmit.Builders;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Activators;

/// <summary>
/// 带参委托激活
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
/// <param name="poco"></param>
/// <param name="activator"></param>
public class DelegateActivator<TSource, TDest>(IPocoOptions poco, Expression<Func<TSource, TDest>> activator)
    : ArgumentFuncCallBuilder(poco, new(typeof(TSource), typeof(TDest)), activator)
    , IEmitActivator
{
    /// <inheritdoc />
    Type IEmitActivator.ReturnType
        => typeof(TDest);
    /// <inheritdoc />
    public Expression New(IBuildContext context, ComplexBuilder builder, Expression argument)
         => Call(argument);
}
