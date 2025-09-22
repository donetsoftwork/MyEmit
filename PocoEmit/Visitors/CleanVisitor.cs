using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PocoEmit.Visitors;

/// <summary>
/// 精简清理访问器
/// </summary>
public class CleanVisitor : ExpressionVisitor
{
    private CleanVisitor() { }
    /// <inheritdoc />
    protected override Expression VisitBinary(BinaryExpression node)
    {
        if (node.NodeType == ExpressionType.Assign)
        {
            var left = node.Left;
            var right = node.Right;
            if (left.NodeType == ExpressionType.Parameter
                && right.NodeType == ExpressionType.Block
                && right is BlockExpression rightBlock)
            {
                var rightResult = rightBlock.Result;
                var rightVariables = rightBlock.Variables;
                // 化简赋值表达式
                // 类型相同左表达式替换参数
                if (rightResult.NodeType == ExpressionType.Parameter
                    && rightResult is ParameterExpression parameter
                    && rightVariables.Contains(parameter)
                    && parameter.Type == left.Type)
                {       
                    var replaceVisitor = new ReplaceVisitor(parameter, left);
                    var assignExpression = replaceVisitor.Visit(rightBlock);
                    if (assignExpression.NodeType == ExpressionType.Block && assignExpression is BlockExpression assignBlock)
                    {
                        if (assignBlock.Result == left)
                        {
                            var expressions = assignBlock.Expressions.Where(item => !item.Equals(left)).ToArray();
                            if (expressions.Length == 1)
                            {
                                assignExpression = expressions[0];
                            }
                            else
                            {
                                var variables = rightVariables.Where(item => !item.Equals(parameter));
                                assignExpression = Expression.Block(variables, expressions);
                            }
                        }
                    }
                    return assignExpression;
                }
            }
        }
        return base.VisitBinary(node);
    }   
    /// <summary>
    /// 实例
    /// </summary>
    public static readonly CleanVisitor Instance = new();
    /// <summary>
    /// 清理
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static Expression Clean(Expression expression)
        => Instance.Visit(expression);
    /// <summary>
    /// 清理
    /// </summary>
    /// <param name="expressions"></param>
    /// <returns></returns>
    public static IEnumerable<Expression> Clean(IEnumerable<Expression> expressions)
        => expressions.Select(Instance.Visit)
            .Where(item => item.NodeType != ExpressionType.Default);
}
