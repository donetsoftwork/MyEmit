using Hand.Creational;
using Hand.Structural;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 包装表达式构造器
/// </summary>
public abstract class WrapExpressionBuilder<TExpression>(ICreator<TExpression> original)
    : ICreator<TExpression>
    , IWrapper<ICreator<TExpression>>
    where TExpression : Expression
{
    #region 配置
    /// <summary>
    /// 原始构造器
    /// </summary>
    protected readonly ICreator<TExpression> _original = original;
    /// <inheritdoc />
    public ICreator<TExpression> Original
        => _original;
    #endregion
    /// <inheritdoc />
    TExpression ICreator<TExpression>.Create()
        => _original.Create();
}
