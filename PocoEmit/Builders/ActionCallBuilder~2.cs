using PocoEmit.Visitors;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 执行Action表达式
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <param name="lambda"></param>
public class ActionCallBuilder<T1, T2>(Expression<Action<T1, T2>> lambda)
{
    #region 配置
    private readonly Expression<Action<T1, T2>> _lambda = lambda;
    /// <summary>
    /// Action表达式
    /// </summary>
    public Expression<Action<T1, T2>> Lambda
        => _lambda;
    #endregion
    /// <summary>
    /// 通过替换参数来调用Action表达式
    /// </summary>
    /// <param name="argument1"></param>
    /// <param name="argument2"></param>
    /// <returns></returns>
    public Expression Call(Expression argument1, Expression argument2)
    {
        var parameters = _lambda.Parameters;
        return new ComplexReplaceVisitor(new ReplaceVisitor(parameters[1], argument2),  parameters[0], argument1)
            .Visit(_lambda.Body);
    }
}
