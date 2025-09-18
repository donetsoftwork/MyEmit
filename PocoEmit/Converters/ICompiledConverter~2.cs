using PocoEmit.Builders;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 强类型转化
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
public interface ICompiledConverter<TSource, TDest> 
    : IEmitConverter
    , IPocoConverter<TSource, TDest>
    , IBuilder<LambdaExpression>
{
    /// <summary>
    /// 类型转化方法
    /// </summary>
    Func<TSource, TDest> ConvertFunc { get; }
}
