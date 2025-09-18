using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 委托构建器
/// </summary>
/// <param name="func"></param>
public class FuncBuilder(LambdaExpression func)
    : IBuilder<Expression>
{
    #region 配置
    private readonly LambdaExpression _func = func;
    /// <summary>
    /// 委托表达式
    /// </summary>
    public LambdaExpression Func 
        => _func;
    #endregion
    /// <inheritdoc />
    public Expression Build()
        => _func.Body;
}
