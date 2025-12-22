using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 表达式构造器
/// </summary>
public interface IEmitBuilder
{
    #region 功能
    /// <summary>
    /// 增加变量
    /// </summary>
    /// <param name="variable">变量</param>
    void AddVariable(ParameterExpression variable);
    /// <summary>
    /// 临时变量
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="initial">初始值</param>
    /// <returns></returns>
    ParameterExpression Temp(Type type, Expression initial);
    /// <summary>
    /// 增加表达式
    /// </summary>
    /// <param name="expression">表达式</param>
    void Add(Expression expression);
    /// <summary>
    /// 构造作用域
    /// </summary>
    /// <returns></returns>
     EmitScopeBuilder CreateScope(List<Expression> expressions);
    /// <summary>
    /// 构造变量作用域
    /// </summary>
    /// <param name="current">当前变量</param>
    /// <param name="expressions"></param>
    /// <returns></returns>
    ScopeVariableBuilder CreateScope(ParameterExpression current, List<Expression> expressions);
    #endregion
}
