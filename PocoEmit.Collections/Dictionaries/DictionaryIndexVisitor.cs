using PocoEmit.Builders;
using PocoEmit.Collections.Visitors;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Dictionaries;

/// <summary>
/// 字典索引访问者
/// </summary>
/// <param name="dictionaryType"></param>
/// <param name="keyType"></param>
/// <param name="elementType"></param>
/// <param name="keysProperty"></param>
/// <param name="itemProperty"></param>
public class DictionaryIndexVisitor(Type dictionaryType, Type keyType, Type elementType, PropertyInfo keysProperty, PropertyInfo itemProperty)
    : EmitDictionaryBase(dictionaryType, keyType, elementType)
    , IElementIndexVisitor
{
    #region 配置
    private readonly PropertyInfo _keysProperty = keysProperty;
    private readonly PropertyInfo _itemProperty = itemProperty;
    #endregion
    /// <inheritdoc />
    Expression IIndexVisitor.Travel(Expression collection, Func<Expression, Expression, Expression> callback)
        => Travel(collection, _keysProperty, _itemProperty, callback);
    /// <summary>
    /// 遍历字典
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="keysProperty"></param>
    /// <param name="itemProperty"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static Expression Travel(Expression dic, PropertyInfo keysProperty, PropertyInfo itemProperty, Func<Expression, Expression, Expression> callback)
    {
        var keysVisitor = CollectionContainer.Instance.VisitorCacher.Get(keysProperty.PropertyType);
        if (keysVisitor is null)
            return Expression.Empty();
        var keys = Expression.Variable(keysProperty.PropertyType, "keys");
        return Expression.Block(
            [keys],
            Expression.Assign(keys, Expression.Property(dic, keysProperty)),
            keysVisitor.Travel(keys, key => callback(key, Expression.MakeIndex(dic, itemProperty, [key])))
        );
    }
}
