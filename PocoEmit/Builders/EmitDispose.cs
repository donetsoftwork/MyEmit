using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Builders;

/// <summary>
/// 释放
/// </summary>
public class EmitDispose
{
    /// <summary>
    /// 释放
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static Expression Dispose(Expression expression)
    {
        var type = expression.Type;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var isDispose = typeof(IDisposable).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());
#else
        var isDispose = typeof(IDisposable).IsAssignableFrom(type);
#endif
        if (isDispose)
        {
            return Expression.Call(expression, _disposeMethod);
        }
        else
        {
            var disposable = Expression.Variable(typeof(IDisposable), "disposable");
            return Expression.Block(
                [disposable],
                Expression.Assign(disposable, Expression.TypeAs(expression, typeof(IDisposable))),
                Expression.IfThen(Expression.NotEqual(disposable, Expression.Constant(null)), Expression.Call(disposable, _disposeMethod))
            );
        }
    }
    #region MethodInfo
    /// <summary>
    /// 获取Dispose方法
    /// </summary>
    private static readonly MethodInfo _disposeMethod = ReflectionHelper.GetMethod(typeof(IDisposable), "Dispose");
    #endregion
}
