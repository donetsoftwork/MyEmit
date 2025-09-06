using PocoEmit.Builders;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Copies;

/// <summary>
/// 委托复制器
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
/// <param name="copyAction"></param>
public class DelegateCopier<TSource, TDest>(Expression<Action<TSource, TDest>> copyAction)
    : ActionCallBuilder<TSource, TDest>(copyAction)
    , IEmitCopier
{
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    /// <inheritdoc />
    public IEnumerable<Expression> Copy(ComplexContext cacher, Expression source, Expression dest)
        => [Call(source, dest)];
}