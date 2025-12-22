using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 变量构造器
/// </summary>
/// <param name="current"></param>
/// <param name="expressions"></param>
public class VariableBuilder(ParameterExpression current, List<Expression> expressions)
    : EmitBuilder([current], expressions)
    , IVariableBuilder
{
    /// <summary>
    /// 变量构造器
    /// </summary>
    /// <param name="current"></param>
    /// <param name="initialization"></param>
    public VariableBuilder(ParameterExpression current, Expression initialization)
        : this(current, [Expression.Assign(current, initialization)])
    {
    }
    #region 配置
    /// <summary>
    /// 当前变量
    /// </summary>
    protected ParameterExpression _current = current;
    /// <summary>
    /// 当前变量
    /// </summary>
    public ParameterExpression Current
        => _current;
    #endregion
    /// <summary>
    /// 更换变量
    /// </summary>
    /// <param name="variable"></param>
    public void Change(ParameterExpression variable)
    {
        if (variable == _current)
            return;
        AddVariable(variable);
        _current = variable;
    }
    #region 简便方法
    /// <summary>
    /// IfDefault
    /// </summary>
    /// <param name="ifTrue"></param>
    /// <param name="ifFalse"></param>
    public void IfDefault(Expression ifTrue, Expression ifFalse)
        => this.IfThenElse(_current, ifTrue, ifFalse);
    /// <summary>
    /// 创建变量构造器
    /// </summary>
    /// <param name="initial"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static VariableBuilder New(Expression initial, string name = null)
    {
        var variable = Expression.Variable(initial.Type, name);
        return new VariableBuilder(variable, initial);
    }
    /// <summary>
    /// 创建变量构造器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="initial"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static VariableBuilder New<T>(T initial, string name = null)
    {
        var variable = Expression.Variable(typeof(T), name);
        return new VariableBuilder(variable, Expression.Constant(initial, typeof(T)));
    }
    #endregion
}
