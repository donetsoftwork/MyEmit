using PocoEmit.Visitors;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 执行Func表达式
/// </summary>
public class FuncCallBuilder<TArgument, TResult>(Expression<Func<TArgument, TResult>> lambda)
{
    #region 配置
    private readonly Expression<Func<TArgument, TResult>> _lambda = lambda;
    /// <summary>
    /// Func表达式
    /// </summary>
    public Expression<Func<TArgument, TResult>> Lambda 
        => _lambda;
    /// <summary>
    /// 返回值类型
    /// </summary>
    public Type ReturnType
        => typeof(TResult);
    #endregion
    /// <summary>
    /// 通过替换参数来调用Func表达式
    /// </summary>
    /// <param name="argument"></param>
    /// <returns></returns>
    public Expression Call(Expression argument)
        => new ReplaceVisitor(_lambda.Parameters[0], argument)
        .Visit(_lambda.Body);
}
