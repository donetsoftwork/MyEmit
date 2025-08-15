using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Collections.Counters;

/// <summary>
/// 按属性获取集合数量
/// </summary>
/// <param name="collectionType"></param>
/// <param name="elementType"></param>
/// <param name="countProperty"></param>
public class PropertyCounter(Type collectionType, Type elementType, PropertyInfo countProperty)
    : EmitCollectionBase(collectionType, elementType)
    , IEmitElementCounter
{
    /// <summary>
    /// 按属性获取集合数量
    /// </summary>
    /// <param name="collectionType"></param>
    /// <param name="elementType"></param>
    public PropertyCounter(Type collectionType, Type elementType)
        : this(collectionType, elementType, GetCountProperty(collectionType) ?? GetCountProperty(typeof(ICollection<>).MakeGenericType(elementType)))
    {
    }
    #region 配置
    private readonly PropertyInfo _countProperty = countProperty;
    /// <summary>
    /// 属性
    /// </summary>
    public PropertyInfo CountProperty 
        => _countProperty;
    /// <inheritdoc />
    bool IEmitCounter.CountByProperty
        => true;
    #endregion
    /// <inheritdoc />
    Expression IEmitCounter.Count(Expression collection)
        => Count(collection);
    /// <summary>
    /// 获取数量
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public Expression Count(Expression instance)
        => Expression.Property(CheckInstance(instance), _countProperty);
    /// <summary>
    /// 按集合类型构造
    /// </summary>
    /// <param name="collectionType"></param>
    /// <returns></returns>   
    public static PropertyCounter CreateByCollectionType(Type collectionType)
    {
        var countProperty = GetCountProperty(collectionType);
        if (countProperty == null)
            return null;
        var elementType = ReflectionHelper.GetElementType(collectionType);
        if (elementType == null)
            return null;
        return new(collectionType, elementType, countProperty);
    }
    /// <summary>
    /// 按集合类型构造
    /// </summary>
    /// <param name="elementType"></param>
    /// <returns></returns>   
    public static PropertyCounter CreateByElementType(Type elementType)
    {
        var collectionType = typeof(ICollection<>).MakeGenericType(elementType);
        return new(collectionType, elementType, GetCountProperty(collectionType));
    }
    /// <summary>
    /// 获取属性Count
    /// </summary>
    /// <param name="collectionType"></param>
    /// <returns></returns>
    public static PropertyInfo GetCountProperty(Type collectionType)
        => ReflectionHelper.GetPropery(collectionType, property => property.Name == "Count" && property.CanRead);
}
