using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 作用域表达式构造器
/// </summary>
public class EmitScopeBuilder(EmitBuilder owner, List<Expression> expressions)
    : EmitBuilderBase(expressions)
    , IEmitBuilder
{
    #region 配置
    private readonly EmitBuilder _owner = owner;
    /// <summary>
    /// 所属构造器
    /// </summary>
    public EmitBuilder Owner 
        => _owner;
    #endregion
    #region IEmitBuilder
    /// <inheritdoc />
    public void AddVariable(ParameterExpression variable)
        => _owner.AddVariable(variable);
    /// <inheritdoc />
    public ParameterExpression Temp(Type type, Expression initial)
    {
        var temp = _owner.CreateTemp(type);
        this.Assign(temp, initial);
        return temp;
    }
    /// <inheritdoc />
    public EmitScopeBuilder CreateScope(List<Expression> expressions)
        => _owner.CreateScope(expressions);
    /// <inheritdoc />
    public ScopeVariableBuilder CreateScope(ParameterExpression current, List<Expression> expressions)
        => _owner.CreateScope(current, expressions);
    #endregion
    /// <summary>
    /// 打包表达式
    /// </summary>
    /// <returns></returns>
    public Expression Create()
    {
        return _expressions.Count switch
        {
            0 => Expression.Empty(),
            1 => _expressions[0],
            _ => Expression.Block([], _expressions),
        };
    }
}
