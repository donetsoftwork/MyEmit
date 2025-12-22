using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 作用域变量构造器
/// </summary>
/// <param name="owner"></param>
/// <param name="current"></param>
/// <param name="expressions"></param>
public class ScopeVariableBuilder(EmitBuilder owner, ParameterExpression current, List<Expression> expressions)
    : EmitScopeBuilder(owner, expressions)
    , IVariableBuilder
{
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
    #endregion
}
