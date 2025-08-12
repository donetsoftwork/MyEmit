using System;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Visitors;

/// <summary>
/// 集合访问者
/// </summary>
public interface ICollectionVisitor
{
    /// <summary>
    /// 集合遍历
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    Expression Travel(Expression collection, Func<Expression, Expression> callback);
}
