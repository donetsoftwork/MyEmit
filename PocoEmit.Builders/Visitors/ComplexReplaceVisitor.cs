using System.Linq.Expressions;

namespace PocoEmit.Visitors;

/// <summary>
/// 复合表达式替换
/// </summary>
/// <param name="inner"></param>
/// <param name="old"></param>
/// <param name="new"></param>
public class ComplexReplaceVisitor(CheckVisitor inner, ParameterExpression old, Expression @new)
    : ReplaceVisitor(old, @new)
{
    #region 配置
    private readonly CheckVisitor _inner = inner;
    /// <summary>
    /// 内部替换访问器
    /// </summary>
    public CheckVisitor Inner 
        => _inner;
    #endregion
    /// <inheritdoc />
    public override Expression Visit(Expression node)
        => _inner.Visit(base.Visit(node));
}
