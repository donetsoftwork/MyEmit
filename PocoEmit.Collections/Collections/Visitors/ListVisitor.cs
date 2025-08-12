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
public class ListVisitor(Type listType, Type elementType)
    : PropertyCounter(listType, elementType)
    , ICollectionVisitor
    , IElementIndexVisitor
{
    /// <summary>
    /// 列表访问者
    /// </summary>
    /// <param name="elementType"></param>
    public ListVisitor(Type elementType)
        : this(typeof(List<>).MakeGenericType(elementType), elementType)
    {
    }
    #region 配置
    private readonly PropertyInfo _itemProperty = GetItemProperty(listType);
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
    public Expression Travel(Expression collection, Func<Expression, Expression> callback)
        => Travel(collection, (_, value) => callback(value));
    /// <summary>
    /// 列表遍历
    /// </summary>
    /// <param name="list"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public Expression Travel(Expression list, Func<Expression, IndexExpression, Expression> callback)
    {
        var index = Expression.Variable(typeof(int), "index");
        var len = Expression.Variable(typeof(int), "len");
        return Expression.Block([index, len],
            Expression.Assign(index, Expression.Constant(0)),
            Expression.Assign(len, Count(list)),
            EmitHelper.For(index, len, i => callback(i, Expression.Property(list, _itemProperty, i)))
        );
    }
    #endregion
    /// <summary>
    /// 获取Item索引器属性
    /// </summary>
    public static PropertyInfo GetItemProperty(Type listType) 
        => ReflectionHelper.GetPropery(listType, property => property.Name == "Item" && property.CanRead && property.CanWrite);
}
