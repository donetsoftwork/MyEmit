using System;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 委托构建器
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <param name="func"></param>
public class FuncBuilder<TValue>(Expression<Func<TValue>> func)
    : IEmitBuilder
{
    #region 配置
    private readonly Expression<Func<TValue>> _func = func;
    /// <summary>
    /// 委托表达式
    /// </summary>
    public Expression<Func<TValue>> Func 
        => _func;
    #endregion
    /// <inheritdoc />
    public Expression Build()
        => _func.Body;
}
