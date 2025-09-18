using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Visitors;

/// <summary>
/// 表达式替换
/// </summary>
/// <param name="old"></param>
/// <param name="new"></param>
public class ReplaceVisitor(ParameterExpression old, Expression @new)
    : CheckVisitor
{
    #region 配置
    private readonly ParameterExpression _old = old;
    private readonly Expression _new = @new;
    /// <summary>
    /// 待替换的表达式
    /// </summary>
    public Expression Old
        => _old;
    /// <summary>
    /// 替换的表达式
    /// </summary>
    protected Expression New
        => _new;
    #endregion
    /// <inheritdoc />
    internal protected override IEnumerable<ParameterExpression> CheckVariables(IEnumerable<ParameterExpression> variables)
    {
        foreach (var expression in variables)
        {
            if (TryReplaceParameter(expression, out var replacement) && replacement is ParameterExpression checkParameter)
                yield return checkParameter;
            else
                yield return expression;
        }
    }
    /// <inheritdoc />
    internal protected override bool TryReplaceParameter(ParameterExpression variable, out Expression replacement)
    {
        if (_old == variable)
        {
            replacement = _new;
            return true;
        }
        replacement = variable;
        return false;
    }
    /// <summary>
    /// 构造表达式替换
    /// </summary>
    /// <param name="reBuild">是否重建</param>
    /// <param name="parameters">形参</param>
    /// <param name="arguments">实参</param>
    /// <returns></returns>
    public static ReplaceVisitor Create(bool reBuild, IList<ParameterExpression> parameters, params Expression[] arguments)
    {
        var count = parameters.Count;
        if (count != arguments.Length)
            throw new ArgumentException("The number of arguments must be {count}");
        if (count == 0)
            return null;

        ReplaceVisitor replaceVisitor;
        if (reBuild)
            replaceVisitor = new ComplexReplaceVisitor(ReBuildVisitor.Empty, parameters[0], arguments[0]);
        else
            replaceVisitor = new ReplaceVisitor(parameters[0], arguments[0]);

        for (int i = 1; i < count; i++)
            replaceVisitor = new ComplexReplaceVisitor(replaceVisitor, parameters[i], arguments[i]);
        return replaceVisitor;
    }
}
