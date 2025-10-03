using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 表达式构造器
/// </summary>
public class EmitBuilder(List<ParameterExpression> variables, List<Expression> expressions)
    : IBuilder<Expression>
{
    /// <summary>
    /// 表达式构造器
    /// </summary>
    public EmitBuilder()
        : this([], [])
    {
    }
    /// <summary>
    /// 复制表达式构造器
    /// </summary>
    /// <param name="builder"></param>
    public EmitBuilder(EmitBuilder builder)
        : this([.. builder._variables], [.. builder._expressions])
    {
    }
    #region 配置
    private readonly List<ParameterExpression> _variables = variables;
    /// <summary>
    /// 变量
    /// </summary>
    public IEnumerable<ParameterExpression> Variables
        => _variables;
    private readonly List<Expression> _expressions = expressions;
    /// <summary>
    /// 表达式
    /// </summary>
    public IEnumerable<Expression> Expressions
        => _expressions;
    #endregion
    /// <inheritdoc />
    public Expression Build()
    {
        return _expressions.Count switch
        {
            0 => Expression.Empty(),
            1 => CheckVariables(_variables, _expressions[0]),
            _ => Expression.Block(_variables, _expressions),
        };
    }
    /// <summary>
    /// 判断变量
    /// </summary>
    /// <param name="variables"></param>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static Expression CheckVariables(List<ParameterExpression> variables, Expression expression)
    {
        if (variables.Count == 0)
            return expression;
        return Expression.Block(variables, expression);
    }
    #region 功能
    /// <summary>
    /// 增加变量
    /// </summary>
    /// <param name="variable"></param>
    public void AddVariable(ParameterExpression variable) 
        => _variables.Add(variable);
    /// <summary>
    /// 增加表达式
    /// </summary>
    /// <param name="expression"></param>
    public void Add(Expression expression)
        => _expressions.Add(expression);
    #endregion
}
