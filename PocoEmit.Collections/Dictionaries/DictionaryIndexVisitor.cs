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
public class DictionaryIndexVisitor(Type dictionaryType, Type keyType, Type elementType)
    : EmitDictionaryBase(dictionaryType, keyType, elementType)
    , IElementIndexVisitor
{
    #region 配置
    private readonly EnumerableVisitor _keysVisitor = new(keyType);
    private readonly PropertyInfo _keysProperty = GetKeysProperty(dictionaryType);
    private readonly PropertyInfo _itemProperty = GetItemProperty(dictionaryType);
    #endregion
    /// <summary>
    /// 遍历字典
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public Expression Travel(Expression collection, Func<Expression, IndexExpression, Expression> callback)
    {
        var keys = Expression.Variable(_keysProperty.PropertyType, "keys");
        var instance = Expression.Variable(_collectionType, "dic");
        return Expression.Block(
            [instance, keys],
            Expression.Assign(instance, CheckInstance(collection)),
            Expression.Assign(keys, Expression.Property(instance, _keysProperty)),
            _keysVisitor.Travel(keys, key => callback(key, Expression.MakeIndex(instance, _itemProperty, [key])))
        );
    }
}
