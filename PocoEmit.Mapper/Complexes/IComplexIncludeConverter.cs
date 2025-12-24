using PocoEmit.Builders;
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
    /// <param name="builder"></param>
    /// <param name="source"></param>
    /// <param name="convertContex"></param>
    Expression BuildFunc(IBuildContext context, IEmitBuilder builder, Expression source, ParameterExpression convertContex);
}
