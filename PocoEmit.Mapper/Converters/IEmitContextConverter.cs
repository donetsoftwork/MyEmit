using PocoEmit.Builders;
using PocoEmit.Configuration;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// Emit上下文转化
/// </summary>
public interface IEmitContextConverter
    : IBuilder<LambdaExpression>
    , IDelegateCompile
    , ICompileInfo
{
    /// <summary>
    /// 转化类型
    /// </summary>
    PairTypeKey Key { get; }
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="context"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    Expression Convert(Expression context, Expression source);
}
