using Hand.Creational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 表达式构造器
/// </summary>
public class EmitBuilder(List<ParameterExpression> variables, List<Expression> expressions)
    : EmitBuilderBase(expressions)
    , IEmitBuilder
    , ICreator<Expression>
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
    /// <summary>
    /// 变量列表
    /// </summary>
    protected readonly List<ParameterExpression> _variables = variables;
    /// <summary>
    /// 临时变量
    /// </summary>
    protected readonly Dictionary<Type, ParameterExpression> _temps = [];
    /// <summary>
    /// 变量列表
    /// </summary>
    public IEnumerable<ParameterExpression> Variables
        => _variables;
    #endregion
    /// <inheritdoc />
    public Expression Create()
    {
        return _expressions.Count switch
        {
            0 => Expression.Empty(),
            1 => CheckVariables(_variables, _expressions[0]),
            _ => Expression.Block(_variables, _expressions),
        };
    }
    /// <summary>
    /// 合并表达式构造器
    /// </summary>
    /// <param name="builder"></param>
    public void Join(EmitBuilder builder)
    {
        foreach(var variable in builder._variables)
            AddVariable(variable);
        _expressions.AddRange(builder._expressions);
    }
    /// <summary>
    /// 打包表达式
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Expression Create(params ParameterExpression[] parameters)
        => Create(CheckVariables(_variables, parameters), _expressions);
    /// <summary>
    /// 打包核心逻辑
    /// </summary>
    /// <param name="variables"></param>
    /// <param name="expressions"></param>
    /// <returns></returns>
    public static Expression Create(IEnumerable<ParameterExpression> variables,  List<Expression> expressions)
    {
        return expressions.Count switch
        {
            0 => Expression.Empty(),
            1 => CheckVariables([.. variables], expressions[0]),
            _ => Expression.Block(variables, expressions),
        };
    }
    /// <summary>
    /// 提取临时变量(排除参数)
    /// </summary>
    /// <param name="list"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    protected static IEnumerable<ParameterExpression> CheckVariables(List<ParameterExpression> list, ParameterExpression[] parameters)
    {
        var count = list.Count;
        if (count == 0)
            return [];
        if (parameters.Length == 0)
            return list;
        return list.Except(parameters);
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
    /// <inheritdoc />
    public void AddVariable(ParameterExpression variable)
    {
        if (_variables.Contains(variable))
            return;
        _variables.Add(variable);
    }
    /// <summary>
    /// 临时变量
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    internal protected virtual ParameterExpression CreateTemp(Type type)
    {
        if (_temps.TryGetValue(type, out var temp))
            return temp;
        temp = this.Declare(type, "t");
        _temps.Add(type, temp);
        return temp;
    }
    /// <inheritdoc />
    public ParameterExpression Temp(Type type, Expression initial)
    {
        var temp = CreateTemp(type);
        this.Assign(temp, initial);
        return temp;
    }
    #endregion
    #region CreateScope
    /// <inheritdoc />
    public EmitScopeBuilder CreateScope(List<Expression> expressions)
        => new(this, expressions);
    /// <inheritdoc />
    public ScopeVariableBuilder CreateScope(ParameterExpression current, List<Expression> expressions)
        => new(this, current, expressions);
    #endregion
}
