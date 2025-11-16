using Hand.Cache;
using Hand.Reflection;
using PocoEmit.Collections.Bundles;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PocoEmit.Collections.Cachers;

/// <summary>
/// 集合成员缓存
/// </summary>
/// <param name="container"></param>
internal class CollectionCacher(CollectionContainer container)
    : CacheFactoryBase<Type, CollectionBundle>(container)
{
    #region 配置
    private readonly CollectionContainer _container = container;
    /// <summary>
    /// 集合容器
    /// </summary>
    public CollectionContainer Container
        => _container;
    #endregion
    /// <inheritdoc />
    protected override CollectionBundle CreateNew(in Type key)
    {
        if (IsCollection(key))
            return CreateByType(_container, key);
        return null;
    }
    /// <summary>
    /// 验证集合类型是否合法
    /// </summary>
    /// <param name="collectionType"></param>
    /// <returns></returns>
    public bool Validate(Type collectionType)
        => Validate(collectionType, out var _);
    /// <summary>
    /// 验证集合类型是否合法
    /// </summary>
    /// <param name="collectionType"></param>
    /// <param name="bundle"></param>
    /// <returns></returns>
    public bool Validate(Type collectionType, out CollectionBundle bundle)
    {
        if (IsCollection(collectionType))
            return (bundle = Get(collectionType)) is not null;
        return TryGetCache(collectionType, out bundle) && bundle is not null;
    }
    /// <summary>
    /// 获取列表成员
    /// </summary>
    /// <param name="container"></param>
    /// <param name="collectionType"></param>
    /// <returns></returns>
    public static CollectionBundle CreateByType(CollectionContainer container, Type collectionType)
    {
        var enumerableInterface = ReflectionType.GetGenericCloseInterfaces(collectionType, typeof(IEnumerable<>))
            .FirstOrDefault();
        if (enumerableInterface is null)
            return null;
        var arguments = ReflectionType.GetGenericArguments(enumerableInterface);
        var elementType = arguments[0];
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var isInterface = collectionType.GetTypeInfo().IsInterface;
#else
        var isInterface = collectionType.IsInterface;
#endif
        ConstructorInfo capacityConstructor = null;
        if (!isInterface)
        {
            capacityConstructor = CollectionContainer.GetCapacityConstructor(collectionType);
        }
        MethodInfo addMethod = GetAddMethod(collectionType, elementType, isInterface);
        var count = CollectionContainer.GetCountProperty(collectionType);
        if(count is null && ReflectionType.HasGenericType(collectionType, typeof(ICollection<>)))
        {
            var collection = container.CollectionCacher.Get(typeof(ICollection<>).MakeGenericType(elementType));
            count = collection.Count;
        }
        return new(isInterface, elementType, capacityConstructor, addMethod, count);
    }
    /// <summary>
    /// 判断是否集合类型
    /// </summary>
    /// <param name="collectionType"></param>
    /// <returns></returns>
    private static bool IsCollection(Type collectionType)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var isCollection = typeof(ICollection).GetTypeInfo().IsAssignableFrom(collectionType.GetTypeInfo());
#else
        var isCollection = typeof(ICollection).IsAssignableFrom(collectionType);
#endif
        if (isCollection || ReflectionType.HasGenericType(collectionType, typeof(ICollection<>)))
            return true;
        return false;
    }
    #region MethodInfo
    /// <summary>
    /// 获取添加方法
    /// </summary>
    /// <param name="collectionType"></param>
    /// <param name="elementType"></param>
    /// <param name="isInterface"></param>
    /// <returns></returns>
    public static MethodInfo GetAddMethod(Type collectionType, Type elementType, bool isInterface)
    {
        if (isInterface)
        {
            if (ReflectionType.HasGenericType(collectionType, typeof(ICollection<>)))
                return CollectionContainer.GetAddMethod(typeof(ICollection<>).MakeGenericType(elementType), elementType);
            if (ReflectionType.HasGenericType(collectionType, typeof(IProducerConsumerCollection<>)))
                return CollectionContainer.GetAddMethod(typeof(IProducerConsumerCollection<>).MakeGenericType(elementType), elementType, "TryAdd");
            return null;
        }
        if (ReflectionType.IsGenericType(collectionType, typeof(List<>)))
            return CollectionContainer.GetAddMethod(collectionType, elementType);
        if (ReflectionType.IsGenericType(collectionType, typeof(HashSet<>)))
            return CollectionContainer.GetAddMethod(collectionType, elementType);
        if (ReflectionType.IsGenericType(collectionType, typeof(Queue<>)))
            return CollectionContainer.GetAddMethod(collectionType, elementType, "Enqueue");
        if (ReflectionType.IsGenericType(collectionType, typeof(Stack<>)))
            return CollectionContainer.GetAddMethod(collectionType, elementType, "Push");
        if (ReflectionType.IsGenericType(collectionType, typeof(BlockingCollection<>)))
            return CollectionContainer.GetAddMethod(collectionType, elementType);
        if (ReflectionType.IsGenericType(collectionType, typeof(ConcurrentQueue<>)))
            return CollectionContainer.GetAddMethod(collectionType, elementType, "Enqueue");
        if (ReflectionType.IsGenericType(collectionType, typeof(ConcurrentStack<>)))
            return CollectionContainer.GetAddMethod(collectionType, elementType, "Push");
        if (ReflectionType.IsGenericType(collectionType, typeof(ConcurrentBag<>)))
            return CollectionContainer.GetAddMethod(collectionType, elementType);
        return CollectionContainer.GetAddMethod(collectionType, elementType);
    }
    #endregion
}
