using PocoEmit.Activators;
using PocoEmit.Collections.Counters;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Collections.Activators;

/// <summary>
/// 集合激活器
/// </summary>
/// <param name="elementType"></param>
/// <param name="collectionType"></param>
/// <param name="sourceCount"></param>
public class CollectionActivator(Type collectionType, Type elementType, IEmitCounter sourceCount)
    : EmitCollectionBase(collectionType, elementType)
    , IEmitActivator
{
    #region 配置
    private readonly IEmitCounter _sourceCount = sourceCount;
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
    public Expression New(Expression argument)
    {
        if (_sourceCount is null)
            return Expression.New(_collectionType);
        // 尝试设置集合的容量,减少扩容的消耗
        var constructor = GetCapacityConstructor(_collectionType);
        if (constructor is null)
            return Expression.New(_collectionType);
        var len = _sourceCount.Count(argument);
        return Expression.New(constructor, len);
    }
    /// <summary>
    /// 获取容量构造函数
    /// </summary>
    /// <param name="collectionType"></param>
    /// <returns></returns>
    private static ConstructorInfo GetCapacityConstructor(Type collectionType)
        => ReflectionHelper.GetConstructor(collectionType, CheckCapacityParameter);
    /// <summary>
    /// 判断容量参数
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    private static bool CheckCapacityParameter(ParameterInfo[] parameters)
    {
        if (parameters.Length == 1)
        {
            var parameter = parameters[0];
            return parameter.Name == "capacity" && parameter.ParameterType == typeof(int);
        }
        return false;
    }
}
