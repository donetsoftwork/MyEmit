using PocoEmit.Configuration;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 执行Func表达式
/// </summary>
/// <param name="poco"></param>
/// <param name="key"></param>
/// <param name="lambda"></param>
public class ArgumentFuncCallBuilder(IPocoOptions poco, PairTypeKey key, LambdaExpression lambda)
    : IBuilder<LambdaExpression>
{
    #region 配置
    private readonly IPocoOptions _poco = poco;
    private readonly PairTypeKey _key = key;
    private readonly LambdaExpression _lambda = lambda;
    /// <summary>
    /// 对象处理
    /// </summary>
    public IPocoOptions Poco
        => _poco;
    /// <inheritdoc />
    public PairTypeKey Key
        => _key;
    /// <summary>
    /// Func表达式
    /// </summary>
    public LambdaExpression Lambda 
        => _lambda;
    #endregion
    /// <inheritdoc />
    LambdaExpression IBuilder<LambdaExpression>.Build()
        => _lambda;
    /// <summary>
    /// 调用Func表达式
    /// </summary>
    /// <param name="argument"></param>
    /// <returns></returns>
    public Expression Call(Expression argument)
        => _poco.Call(_lambda, argument);
}
