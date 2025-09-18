using PocoEmit.Activators;
using PocoEmit.Collections.Counters;
using PocoEmit.Complexes;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Collections.Activators;

/// <summary>
/// 集合激活器
/// </summary>
/// <param name="collectionType"></param>
/// <param name="elementType"></param>
/// <param name="capacityConstructor"></param>
/// <param name="sourceCount"></param>
public class CollectionActivator(Type collectionType, Type elementType, ConstructorInfo capacityConstructor, IEmitCounter sourceCount)
    : EmitCollectionBase(collectionType, elementType)
    , IEmitActivator
{
    #region 配置
    private readonly ConstructorInfo _capacityConstructor = capacityConstructor;
    private readonly IEmitCounter _sourceCount = sourceCount;
    /// <summary>
    /// 容量构造函数
    /// </summary>
    public ConstructorInfo CapacityConstructo
        => _capacityConstructor;
    /// <summary>
    /// 获取源数据量
    /// </summary>
    public IEmitCounter SourceCount
        => _sourceCount;
    /// <inheritdoc />
    Type IEmitActivator.ReturnType
        => _collectionType;
    #endregion
    /// <inheritdoc />
    public Expression New(IBuildContext context, Expression argument)
    {
        if (_sourceCount is null)
            return Expression.New(_collectionType);
        // 尝试设置集合的容量,减少扩容的消耗
        if (_capacityConstructor is null)
            return Expression.New(_collectionType);
        var len = _sourceCount.Count(argument);
        return Expression.New(_capacityConstructor, len);
    }
}
