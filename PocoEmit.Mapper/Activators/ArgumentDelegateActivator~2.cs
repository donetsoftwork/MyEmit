using PocoEmit.Builders;
using PocoEmit.Converters;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Activators;

/// <summary>
/// 带参委托激活
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
/// <param name="activator"></param>
public class DelegateActivator<TSource, TDest>(Expression<Func<TSource, TDest>> activator)
    : FuncCallBuilder<TSource, TDest>(activator)
    , IEmitActivator
{
    /// <inheritdoc />
    public Expression New(ComplexContext cacher, Expression argument)
         => Call(argument);
}
