using System;
using System.Linq;
using System.Reflection;

namespace PocoEmit.Collections.Counters;

/// <summary>
/// 获取迭代器数量
/// </summary>
public class EnumerableCounter(Type collectionType, Type elementType)
    : MethodCounter(collectionType, elementType, null, EnumerableCountMethod.MakeGenericMethod(elementType)) 
    , IEmitCollectionCounter
{
    /// <summary>
    /// Count扩展方法
    /// </summary>
    public static readonly MethodInfo EnumerableCountMethod = ReflectionHelper.GetMethods(typeof(Enumerable))
        .First(m => m.Name == "Count" && m.GetParameters().Length == 1);
}
