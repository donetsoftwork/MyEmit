using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Complexes;

/// <summary>
/// 可能包含复杂类型转化
/// </summary>
public interface IComplexIncludeConverter
     : IComplexPreview     
{
    /// <summary>
    /// 转化核心方法
    /// </summary>
    /// <param name="context"></param>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <param name="convertContext"></param>
    /// <returns></returns>
    IEnumerable<Expression> BuildBody(IBuildContext context, Expression source, Expression dest, ParameterExpression convertContext);
}
