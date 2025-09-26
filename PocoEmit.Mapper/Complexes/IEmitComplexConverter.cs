using PocoEmit.Converters;
using System.Linq.Expressions;

namespace PocoEmit.Complexes;

/// <summary>
/// Emit复杂类型转化
/// </summary>
public interface IEmitComplexConverter
    : IComplexIncludeConverter
    , IEmitConverter
{
    ///// <summary>
    ///// 转化
    ///// </summary>
    ///// <param name="context">构建上下文</param>
    ///// <param name="source">源表达式</param>
    ///// <returns></returns>
    //Expression Convert(IBuildContext context, Expression source);
    /// <summary>
    /// 构造表达式
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    LambdaExpression Build(IBuildContext context);
    /// <summary>
    /// 构造上下文表达式
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    LambdaExpression BuildWithContext(IBuildContext context);
}
