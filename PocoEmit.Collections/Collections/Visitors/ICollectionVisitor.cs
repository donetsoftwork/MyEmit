using System;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Visitors;

/// <summary>
/// 集合元素访问者
/// </summary>
public interface IEmitElementVisitor : IEmitCollection
{
    /// <summary>
    /// 集合遍历
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    Expression Travel(Expression collection, Func<Expression, Expression> callback);
}
