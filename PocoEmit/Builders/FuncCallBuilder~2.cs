using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 执行Func表达式
/// </summary>
/// <param name="poco"></param>
/// <param name="key"></param>
/// <param name="lambda"></param>
public class ArgumentFuncCallBuilder(IPocoOptions poco, in PairTypeKey key, LambdaExpression lambda)
    : IBuilder<LambdaExpression>
{
    #region 配置
    /// <summary>
    /// 对象处理
    /// </summary>
    protected readonly IPocoOptions _poco = poco;
    /// <summary>
    /// 
    /// </summary>
    protected readonly PairTypeKey _key = key;
    private LambdaExpression _lambda = lambda;
    /// <summary>
    /// 对象处理
    /// </summary>
    public IPocoOptions Poco
        => _poco;
    /// <summary>
    /// 转化类型
    /// </summary>
    public PairTypeKey Key
        => _key;
    /// <summary>
    /// Func表达式
    /// </summary>
    public LambdaExpression Lambda 
        => _lambda;
    #endregion
    /// <summary>
    /// 构建表达式
    /// </summary>
    /// <param name="lambda"></param>
    public void Build(LambdaExpression lambda)
        => _lambda = lambda ?? throw new ArgumentNullException(nameof(lambda));
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
