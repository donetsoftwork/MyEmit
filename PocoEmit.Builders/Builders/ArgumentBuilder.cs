using Hand.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 带参表达式构造器
/// </summary>
/// <param name="argument">源表达式</param>
public class ArgumentBuilder(ParameterExpression argument)
    : VariableBuilder(argument, [])
{
    #region 配置
    private readonly ParameterExpression _argument = argument;
    /// <summary>
    /// 参数表达式
    /// </summary>
    public ParameterExpression Argument
        => _argument;
    #endregion
    /// <summary>
    /// 构造Action
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Expression CreateAction(params ParameterExpression[] parameters)
    {
        var body = Create(parameters);
        var argumentType = _argument.Type;
        if (PairTypeKey.CheckNullCondition(argumentType))
        {
            return Expression.IfThen(
                Expression.NotEqual(_argument, Expression.Constant(null, argumentType)),
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
        var argumentType = _argument.Type;
        var variables = CheckVariables(_variables, parameters);        
        if (PairTypeKey.CheckNullCondition(argumentType))
        {
            var body = Create([], _expressions);
            return Expression.Block(
                variables,
                Expression.IfThenElse(
                    Expression.Equal(_argument, Expression.Constant(null, argumentType)),
                    Expression.Assign(result, Expression.Default(destType)),
                    body
                    ),
                result
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
