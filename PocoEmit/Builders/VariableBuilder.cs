using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 变量构造器
/// </summary>
/// <param name="variable"></param>
/// <param name="expressions"></param>
public class VariableBuilder(ParameterExpression variable, List<Expression> expressions)
    : EmitBuilder([variable], expressions)
{
    /// <summary>
    /// 变量构造器
    /// </summary>
    /// <param name="variable"></param>
    /// <param name="initialization"></param>
    public VariableBuilder(ParameterExpression variable, Expression initialization)
        : this(variable, [Expression.Assign(variable, initialization)])
    {
    }
    #region 配置
    private readonly ParameterExpression _variable = variable;
    /// <summary>
    /// 变量
    /// </summary>
    public ParameterExpression Variable
        => _variable;
    #endregion
    /// <summary>
    /// 判null初始化
    /// </summary>
    /// <param name="expression"></param>
    public void AddIfNull(Expression expression)
        => Add(Expression.IfThen(Expression.Equal(_variable, Expression.Constant(null)), expression));
    /// <summary>
    /// 判null初始化
    /// </summary>
    /// <param name="expression"></param>
    public void AddIfNotNull(Expression expression)
        => Add(Expression.IfThen(Expression.NotEqual(_variable, Expression.Constant(null)), expression));
    /// <summary>
    /// 为null才赋值
    /// </summary>
    /// <param name="expression"></param>
    public void AssignIfNull(Expression expression)
        => AddIfNull(Expression.Assign(_variable, expression));
}
