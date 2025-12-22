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
    : ArrayCounter(arrayType, elementType)
    , IEmitElementVisitor
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
    /// <inheritdoc />
    Type IElementIndexVisitor.KeyType
        => typeof(int);
    #endregion
    /// <inheritdoc />
    Expression IIndexVisitor.Travel(IEmitBuilder builder, Expression collection, Func<Expression, Expression, Expression> callback)
        => Travel(builder, collection, callback);
    /// <inheritdoc />
    Expression IEmitElementVisitor.Travel(IEmitBuilder builder, Expression collection, Func<Expression, Expression> callback)
        => Travel(builder, collection, (_, item) => callback(item));
    #region Travel
    /// <summary>
    /// 数组遍历
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="array"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static Expression Travel(IEmitBuilder builder, Expression array, Func<Expression, Expression> callback)
        => Travel(builder, array, (_, item) => callback(item));
    /// <summary>
    /// 数组遍历
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="array"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static Expression Travel(IEmitBuilder builder, Expression array, Func<Expression, IndexExpression, Expression> callback)
    {
        var count = builder.Declare<int>("count");
        var index = builder.Declare<int>("index");
        builder.Assign(count, Expression.ArrayLength(array));
        builder.Assign(index, Expression.Constant(0));
        return EmitHelper.For(index, count, i => callback(i, Expression.ArrayAccess(array, i)));
    }
    #endregion
}
