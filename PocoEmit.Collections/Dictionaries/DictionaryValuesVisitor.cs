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
/// <param name="valuesProperty"></param>
public class DictionaryValuesVisitor(Type dictionaryType, Type elementType, PropertyInfo valuesProperty)
    : EmitCollectionBase(dictionaryType, elementType)
    , IEmitElementVisitor
{
    #region 配置
    //private readonly EnumerableVisitor _valuesVisitor = new(elementType);
    private readonly PropertyInfo _valuesProperty = valuesProperty;
    #endregion
    /// <inheritdoc />
    Expression IEmitElementVisitor.Travel(Expression collection, Func<Expression, Expression> callback)
        => Travel(collection, _valuesProperty/*, _valuesVisitor*/, callback);
    /// <summary>
    /// 遍历字典Values
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static Expression Travel(Expression dic, Func<Expression, Expression> callback)
    {
        var dictionaryType = dic.Type;
        var bundle = CollectionContainer.Instance.DictionaryCacher.Get(dictionaryType);
        var valuesProperty = bundle?.Values;
        if (valuesProperty is null)
            return Expression.Empty();
        //EnumerableVisitor valuesVisitor = new(dictionaryType);
        return Travel(dic, valuesProperty/*, valuesVisitor*/, callback);
    }
    /// <summary>
    /// 遍历字典Values
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="valuesProperty"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static Expression Travel(Expression dic, PropertyInfo valuesProperty/*, IEmitElementVisitor valuesVisitor*/, Func<Expression, Expression> callback)
    {
        //IEmitElementVisitor valuesVisitor = CollectionContainer.Instance.VisitorCacher.Get(valuesProperty.PropertyType);
        //if(valuesVisitor is null) 
        //    return Expression.Empty();    
        var values = Expression.Variable(valuesProperty.PropertyType, "values");
        //var instance = Expression.Variable(_collectionType, "dic");
        return Expression.Block(
            [values],
            //Expression.Assign(instance, CheckInstance(collection)),
            Expression.Assign(values, Expression.Property(dic, valuesProperty)),
            EnumerableVisitor.Travel(values, value => callback(value))
        );
    }
}
