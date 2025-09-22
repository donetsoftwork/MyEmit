using PocoEmit.Resolves;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 上线文转化
/// </summary>
public interface IContextConverter
{
    /// <summary>
    /// 表达式
    /// </summary>
    LambdaExpression Lambda { get; }
    /// <summary>
    /// 委托
    /// </summary>
    Delegate Func { get; }
    /// <summary>
    /// 转化
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <returns></returns>
    TDest Convert<TSource, TDest>(IConvertContext context, TSource source);
}
