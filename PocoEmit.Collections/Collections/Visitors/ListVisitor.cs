using PocoEmit.Builders;
using PocoEmit.Collections.Counters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Collections.Visitors;

/// <summary>
/// 列表访问者
/// </summary>
/// <param name="listType"></param>
/// <param name="elementType"></param>
/// <param name="countProperty"></param>
/// <param name="itemProperty"></param>
public class ListVisitor(Type listType, Type elementType, PropertyInfo countProperty, PropertyInfo itemProperty)
    : PropertyCounter(listType, elementType, countProperty)
    , IEmitElementVisitor
    , IElementIndexVisitor
{
    #region 配置
    private readonly PropertyInfo _itemProperty = itemProperty;
    /// <summary>
    /// 索引器属性
    /// </summary>
    public PropertyInfo ItemProperty
        => _itemProperty;
    /// <inheritdoc />
    Type IElementIndexVisitor.KeyType
        => typeof(int);
    #endregion
    #region Travel
    /// <inheritdoc />
    Expression IEmitElementVisitor.Travel(IEmitBuilder builder, Expression collection, Func<Expression, Expression> callback)
        => Travel(builder, collection, _countProperty, _itemProperty, (_, value) => callback(value));
    /// <inheritdoc />
    Expression IIndexVisitor.Travel(IEmitBuilder builder, Expression collection, Func<Expression, Expression, Expression> callback)
        => Travel(builder, collection, _countProperty, _itemProperty, callback);
    /// <summary>
    /// 遍历
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="list"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static Expression Travel(IEmitBuilder builder, Expression list, Func<Expression, Expression, Expression> callback)
    {
        var collectionType = list.Type;
        var bundle = CollectionContainer.Instance.ListCacher.Get(collectionType);
        if (bundle == null)
            return Expression.Empty();
        return Travel(builder, list, bundle.Count, bundle.Items, callback);
    }
    /// <summary>
    /// 遍历
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="list"></param>
    /// <param name="count"></param>
    /// <param name="item"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static Expression Travel(IEmitBuilder builder, Expression list, PropertyInfo count, PropertyInfo item, Func<Expression, Expression, Expression> callback)
    {
        var index = builder.Declare<int>("index");
        var len = builder.Declare<int>("len");
        builder.Assign(index, Expression.Constant(0));
        builder.Assign(len, Expression.Property(list, count));
        return EmitHelper.For(index, len, i => callback(i, Expression.Property(list, item, i)));
    }
    #endregion
}
