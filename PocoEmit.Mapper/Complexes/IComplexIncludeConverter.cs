using PocoEmit.Builders;
using PocoEmit.Converters;
using System.Linq.Expressions;

namespace PocoEmit.Complexes;

/// <summary>
/// 可能包含复杂类型转化
/// </summary>
public interface IComplexIncludeConverter
     : IComplexPreview     
{
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="context">构建上下文</param>
    /// <param name="source">源表达式</param>
    /// <returns></returns>
    Expression Convert(IBuildContext context, Expression source);
}
