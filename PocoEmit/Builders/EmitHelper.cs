using System;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// Emit工具
/// </summary>
public static class EmitHelper
{
    #region CheckMethodCallInstance
    /// <summary>
    /// 检查调用委托目标
    /// </summary>
    /// <param name="delegate"></param>
    /// <returns></returns>
    public static Expression CheckMethodCallInstance(Delegate @delegate)
        => CheckMethodCallInstance(@delegate.Target);
    /// <summary>
    /// 检查调用对象
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public static Expression CheckMethodCallInstance(object instance)
    {
        if (instance is null)
            return null;
        return Expression.Constant(instance);
    }
    #endregion
    #region For
    /// <summary>
    /// for循环
    /// </summary>
    /// <param name="count"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    public static BlockExpression For(Expression count, Func<Expression, Expression> callBack)
    {
        var index = Expression.Variable(typeof(int), "index");
        return Expression.Block(
            [index],
            Expression.Assign(index, Expression.Constant(0, typeof(int))),
            For(index, count, callBack)
            );
    }
    /// <summary>
    /// for循环
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    public static LoopExpression For(Expression index, Expression count, Func<Expression, Expression> callBack)
    {
        var forLabel = Expression.Label("forLabel");
        return Expression.Loop(
            Expression.IfThenElse(
                Expression.LessThan(index, count),
                Expression.Block(callBack(index), Expression.PostIncrementAssign(index)),
                Expression.Break(forLabel)
            ),
            forLabel);
    }
    #endregion
}
