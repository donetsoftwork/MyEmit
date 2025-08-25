using PocoEmit.Builders;
using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 委托表达式类型转化
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
/// <param name="convertFunc"></param>
public sealed class DelegateConverter<TSource, TDest>(Expression<Func<TSource, TDest>> convertFunc)
    : FuncCallBuilder<TSource, TDest>(convertFunc)
    , IEmitConverter
{
    #region 配置
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Convert(Expression source)
        => Call(source);
}
