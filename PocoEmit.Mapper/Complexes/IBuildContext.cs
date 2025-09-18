using PocoEmit.Configuration;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Complexes;

/// <summary>
/// 构建器上下文
/// </summary>
public interface IBuildContext
{
    /// <summary>
    /// 构建器上下文
    /// </summary>
    BuildContext Context { get; }
    /// <summary>
    /// 执行上下文
    /// </summary>
    List<ParameterExpression> ConvertContexts { get; }
    /// <summary>
    /// 获取复杂类型成员信息
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    ComplexBundle GetBundle(PairTypeKey key);
    /// <summary>
    /// 调用
    /// </summary>
    /// <param name="lambda"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    Expression Call(LambdaExpression lambda, params Expression[] arguments);
    /// <summary>
    /// 赋值初始化执行上线文
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Expression InitContext(ParameterExpression context);
    /// <summary>
    /// 获取表达式
    /// </summary>
    /// <param name="key"></param>
    /// <param name="lambda"></param>
    /// <returns></returns>
    bool TryGetLambda(PairTypeKey key, out LambdaExpression lambda);
    /// <summary>
    /// 获取上下文表达式
    /// </summary>
    /// <param name="key"></param>
    /// <param name="lambda"></param>
    /// <returns></returns>
    bool TryGetContextLambda(PairTypeKey key, out LambdaExpression lambda);
    /// <summary>
    /// 保存上下文表达式
    /// </summary>
    /// <param name="key"></param>
    /// <param name="lambda"></param>
    bool SetContextLambda(PairTypeKey key, LambdaExpression lambda);
}
