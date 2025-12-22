using Hand.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 复杂对象表达式构造器
/// </summary>
/// <param name="source">源表达式</param>
public class ComplexBuilder(ParameterExpression source)
    : VariableBuilder(source, [])
{
    #region 配置
    private readonly ParameterExpression _source = source;
    /// <summary>
    /// 源表达式
    /// </summary>
    public ParameterExpression Source
        => _source;
    #endregion
    /// <summary>
    /// 构造Action
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Expression CreateAction(params ParameterExpression[] parameters)
    {
        var body = Create(parameters);
        var sourceType = _source.Type;
        if (PairTypeKey.CheckNullCondition(sourceType))
        {
            return Expression.IfThen(
                Expression.NotEqual(_source, Expression.Constant(null, sourceType)),
                body
            );
        }
        return body;
    }
    /// <summary>
    /// 构造Func
    /// </summary>
    /// <param name="result"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Expression CreateFunc(Expression result, params ParameterExpression[] parameters)
    {
        var destType = result.Type;
        var sourceType = _source.Type;
        var variables = CheckVariables(_variables, parameters);        
        if (PairTypeKey.CheckNullCondition(sourceType))
        {
            var body = CreateCore(result, _expressions);
            return Expression.Block(
                variables,
                Expression.Condition(
                    Expression.Equal(_source, Expression.Constant(null, sourceType)),
                    Expression.Default(destType),
                    body,
                    destType
                    )
                );
        }
        else
        {
            var variableList = variables.ToArray();
            if(variableList.Length == 0)
                return CreateCore(result, _expressions);
            return Expression.Block(variableList, [.. _expressions, result]);
        }
    }
    /// <summary>
    /// 打包核心逻辑
    /// </summary>
    /// <param name="result"></param>
    /// <param name="expressions"></param>
    /// <returns></returns>
    private static Expression CreateCore(Expression result, List<Expression> expressions)
    {
        return expressions.Count switch
        {
            0 => result,
            _ => Expression.Block([], [.. expressions, result]),
        };
    }
}
