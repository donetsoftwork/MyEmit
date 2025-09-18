using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Visitors;

/// <summary>
/// 表达式检查处理
/// </summary>
public abstract class CheckVisitor : ExpressionVisitor
{
    /// <summary>
    /// 尝试替换参数
    /// </summary>
    /// <param name="variable"></param>
    /// <param name="replacement"></param>
    /// <returns></returns>
    internal protected abstract bool TryReplaceParameter(ParameterExpression variable, out Expression replacement);
    /// <summary>
    /// 尝试替换
    /// </summary>
    /// <param name="variable"></param>
    /// <param name="replacement"></param>
    /// <returns></returns>
    protected bool TryReplace(Expression variable, out Expression replacement)
    {
        if(variable is null)
        {
            replacement = variable;
            return false;
        }
        if(variable.NodeType == ExpressionType.Parameter && variable is ParameterExpression parameter)
            return TryReplaceParameter(parameter, out replacement);
        replacement = variable;
        return false;
    }
    /// <summary>
    /// 遍历表达式
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public override Expression Visit(Expression node)
    {
        if(node is null) 
            return node;
        // 尝试替换
        if (TryReplace(node, out var replacement))
            return replacement;
        return VisitCore(node);
    }
    /// <inheritdoc />
    protected override Expression VisitParameter(ParameterExpression node)
    {
        TryReplaceParameter(node, out var replacement);
        return replacement;
    }
    /// <inheritdoc />
    protected override Expression VisitBlock(BlockExpression node)
        => CheckBlock(node);
    /// <summary>
    /// 检查BlockExpression
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    internal protected virtual Expression CheckBlock(BlockExpression node)
        => Expression.Block(CheckVariables(node.Variables), CheckExpressions(node.Expressions));
    /// <summary>
    /// 默认处理
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    internal protected virtual Expression VisitCore(Expression node)
        => base.Visit(node);
    /// <summary>
    /// 处理变量(参数)
    /// </summary>
    /// <param name="variables"></param>
    /// <returns></returns>
    internal protected abstract IEnumerable<ParameterExpression> CheckVariables(IEnumerable<ParameterExpression> variables);
    /// <summary>
    /// 检查表达式子列表
    /// </summary>
    /// <param name="expressions"></param>
    /// <returns></returns>
    public IEnumerable<Expression> CheckExpressions(IEnumerable<Expression> expressions)
    {
        foreach (var expression in expressions)
        {
            if (TryReplace(expression, out var replacement))
                yield return replacement;
            else
                yield return Visit(expression);
        }
    }
}
