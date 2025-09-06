using PocoEmit.Converters;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Activators;

/// <summary>
/// 表达式激活
/// </summary>
/// <param name="expression"></param>
/// <param name="returnType"></param>
public class ExpressionActivator(Expression expression, Type returnType)
    : IEmitActivator
{
    #region 配置
    /// <summary>
    /// 表达式
    /// </summary>
    protected readonly Expression _expression = expression;
    /// <summary>
    /// 表达式
    /// </summary>
    public Expression Expression
        => _expression;
    private readonly Type _returnType = returnType;
    /// <inheritdoc />
    public Type ReturnType
        => _returnType;
    #endregion
    /// <inheritdoc />
    public virtual Expression New(ComplexContext cacher, Expression argument)
        => _expression;
}
