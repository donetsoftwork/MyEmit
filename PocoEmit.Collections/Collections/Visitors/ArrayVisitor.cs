using PocoEmit.Builders;
using PocoEmit.Collections.Counters;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Visitors;

/// <summary>
/// 数组访问者
/// </summary>
/// <param name="arrayType"></param>
/// <param name="elementType"></param>
public class ArrayVisitor(Type arrayType, Type elementType)
    : ArrayCounter(arrayType)
    , ICollectionVisitor
    , IElementIndexVisitor
{
    /// <summary>
    /// 数组访问者
    /// </summary>
    /// <param name="arrayType"></param>
    public ArrayVisitor(Type arrayType)
        : this(arrayType, arrayType.GetElementType())
    {
    }
    #region 配置
    private readonly Type _elementType = elementType;
    /// <inheritdoc />
    public Type ElementType
        => _elementType;
    /// <inheritdoc />
    Type IElementIndexVisitor.KeyType
        => typeof(int);
    #endregion
    /// <inheritdoc />
    Expression IIndexVisitor.Travel(Expression collection, Func<Expression, IndexExpression, Expression> callback)
        => Travel(collection, callback);
    /// <inheritdoc />
    Expression ICollectionVisitor.Travel(Expression collection, Func<Expression, Expression> callback)
        => Travel(collection, (_, item) => callback(item));
    #region Travel
    /// <summary>
    /// 数组遍历
    /// </summary>
    /// <param name="array"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static Expression Travel(Expression array, Func<Expression, Expression> callback)
        => Travel(array, (_, item) => callback(item));
    /// <summary>
    /// 数组遍历
    /// </summary>
    /// <param name="array"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static Expression Travel(Expression array, Func<Expression, IndexExpression, Expression> callback)
    {
        var index = Expression.Variable(typeof(int), "index");
        var len = Expression.Variable(typeof(int), "len");
        return Expression.Block([index, len],
            Expression.Assign(index, Expression.Constant(0)),
            Expression.Assign(len, Expression.ArrayLength(array)),
            EmitHelper.For(index, len, i => callback(i, Expression.ArrayAccess(array, i)))
        );
    }
    #endregion
    ///// <summary>
    ///// 读取
    ///// </summary>
    ///// <param name="array"></param>
    ///// <param name="index"></param>
    ///// <returns></returns>
    //public static Expression Read(Expression array, Expression index)
    //    => Expression.ArrayAccess(array, index);
    ///// <summary>
    ///// 写入
    ///// </summary>
    ///// <param name="array"></param>
    ///// <param name="index"></param>
    ///// <param name="value"></param>
    ///// <returns></returns>
    //public static Expression Write(Expression array, Expression index, Expression value)
    //    => Expression.Assign(Expression.ArrayAccess(array, index), value);
}
