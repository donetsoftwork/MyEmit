using PocoEmit.Collections;
using PocoEmit.Collections.Visitors;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Dictionaries;

/// <summary>
/// 字典值遍历
/// </summary>
/// <param name="dictionaryType"></param>
/// <param name="elementType"></param>
public class DictionaryValuesVisitor(Type dictionaryType, Type elementType)
    : EmitCollectionBase(dictionaryType, elementType)
    , ICollectionVisitor
{
    #region 配置
    private readonly EnumerableVisitor _valuesVisitor = new(elementType);
    private readonly PropertyInfo _valuesProperty = EmitDictionaryBase.GetValuesProperty(dictionaryType);
    #endregion
    /// <summary>
    /// 遍历字典
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public Expression Travel(Expression collection, Func<Expression, Expression> callback)
    {
        var values = Expression.Variable(_valuesProperty.PropertyType, "values");
        var instance = Expression.Variable(_collectionType, "dic");
        return Expression.Block(
            [instance, values],
            Expression.Assign(instance, CheckInstance(collection)),
            Expression.Assign(values, Expression.Property(instance, _valuesProperty)),
            _valuesVisitor.Travel(values, value => callback(value))
        );
    }
}
