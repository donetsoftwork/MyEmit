using System;
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
    #region 配置
    /// <summary>
    /// Count属性
    /// </summary>
    protected readonly PropertyInfo _countProperty = countProperty;
    /// <summary>
    /// Count属性
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
}
