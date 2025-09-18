using PocoEmit.Builders;
using PocoEmit.Configuration;
using PocoEmit.Visitors;
using System.Linq.Expressions;

namespace PocoEmit.CollectionsUnitTests.Supports;

/// <summary>
/// 复杂类型转化表达式构造器
/// </summary>
/// <param name="source"></param>
/// <param name="dest"></param>
/// <param name="bodies"></param>
public class ConvertExpressionBuilder(ParameterExpression source, ParameterExpression dest, IEnumerable<Expression> bodies)
    : IBuilder<LambdaExpression>
{
    #region 配置
    private readonly ParameterExpression _source = source;
    private readonly ParameterExpression _dest = dest;
    private readonly IEnumerable<Expression> _bodies = bodies;
    private readonly LambdaExpression _lambda = BuildLambda(source, dest, bodies);
    /// <summary>
    /// 源表达式
    /// </summary>
    public ParameterExpression Source 
        => _source;
    /// <summary>
    /// 目标表达式
    /// </summary>
    public ParameterExpression Dest 
        => _dest;
    /// <summary>
    /// 主体表达式
    /// </summary>
    public IEnumerable<Expression> Bodies
        => _bodies;
    /// <summary>
    /// 委托表达式
    /// </summary>
    public LambdaExpression Lambda 
        => _lambda;
    #endregion
    LambdaExpression IBuilder<LambdaExpression>.Build()
        => _lambda;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="argument"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public Expression Convert(Expression argument, Expression target)
    {
        ComplexReplaceVisitor visitor;
        List<ParameterExpression> variables = [];
        List<Expression> expressions = [];
        if(EmitHelper.CheckComplexSource(argument, false))
        {
            var source = Expression.Parameter(_source.Type, _source.Name);
            variables.Add(source);
            visitor = new ComplexReplaceVisitor(ReBuildVisitor.Empty, _source, source);
            expressions.Add(Expression.Assign(source, argument));
        }
        else
        {
            visitor = new ComplexReplaceVisitor(ReBuildVisitor.Empty, _source, argument);
        }
        Expression targetAssign = null;
        if (EmitHelper.CheckComplexSource(target, false))
        {
            var dest = Expression.Parameter(_dest.Type, _dest.Name);
            variables.Add(dest);
            visitor = new ComplexReplaceVisitor(visitor, _dest, dest);
            targetAssign = Expression.Assign(target, dest);
        }
        else
        {
            visitor = new ComplexReplaceVisitor(visitor, _dest, target);
        }
        foreach (var expression in _bodies)
        {
            expressions.Add(visitor.Visit(expression));
        }
        if (targetAssign is not null)
            expressions.Add(targetAssign);
        if (expressions.Count > 0)
            return Expression.Block(variables, expressions);
        return Expression.Empty();
    }
    #region BuildLambda
    /// <summary>
    /// 构造委托表达式
    /// </summary>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <param name="bodies"></param>
    /// <returns></returns>
    public static LambdaExpression BuildLambda(ParameterExpression source, ParameterExpression dest, IEnumerable<Expression> bodies)
    {
        var sourceType = source.Type;
        var destType = dest.Type;
        BlockExpression body;
        if (PairTypeKey.CheckNullCondition(sourceType))
        {
            body = Expression.Block(
                [dest],
                Expression.IfThen(
                        Expression.NotEqual(source, Expression.Constant(null, sourceType)),
                        Expression.Block(bodies)
                ),
                dest);
        }
        else
        {
            var list = new List<Expression>(bodies)
            {
                dest
            };
            body = Expression.Block([dest], list);
        }
        return Expression.Lambda(Expression.GetFuncType(sourceType, destType), body, source);
    }
    #endregion
}
