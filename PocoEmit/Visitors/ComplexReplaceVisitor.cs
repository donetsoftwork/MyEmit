using System.Linq.Expressions;

namespace PocoEmit.Visitors;

/// <summary>
/// 复合表达式替换
/// </summary>
/// <param name="inner"></param>
/// <param name="old"></param>
/// <param name="new"></param>
internal class ComplexReplaceVisitor(ReplaceVisitor inner, Expression old, Expression @new)
    : ReplaceVisitor(old, @new)
{
    #region 配置
    private readonly ReplaceVisitor _inner = inner;
    /// <summary>
    /// 内部替换访问器
    /// </summary>
    public ReplaceVisitor Inner 
        => _inner;
    #endregion
    /// <inheritdoc />
    protected override Expression VisitCore(Expression node)
        => _inner.Visit(node);
}
