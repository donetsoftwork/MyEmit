using System.Linq.Expressions;

namespace PocoEmit.Visitors;

/// <summary>
/// 表达式替换
/// </summary>
/// <param name="old"></param>
/// <param name="new"></param>
internal class ReplaceVisitor(Expression old, Expression @new)
    : ExpressionVisitor
{
    #region 配置
    private readonly Expression _old = old;
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
    /// <summary>
    /// 遍历表达式
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public override Expression Visit(Expression node)
    {
        // 替换表达式
        if (node == _old)
            return base.Visit(_new);
        return VisitCore(node);
    }
    /// <summary>
    /// 原始遍历方法
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    protected virtual Expression VisitCore(Expression node)
        => base.Visit(node);
}
