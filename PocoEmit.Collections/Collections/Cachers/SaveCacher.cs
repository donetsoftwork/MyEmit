using PocoEmit.Collections.Saves;
using PocoEmit.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
using System.Reflection;
#endif

namespace PocoEmit.Collections.Cachers;

/// <summary>
/// 元素保存器缓存
/// </summary>
/// <param name="cacher"></param>
internal class SaveCacher(ICacher<PairTypeKey, IEmitElementSaver> cacher)
   : CacheBase<PairTypeKey, IEmitElementSaver>(cacher)
{
    /// <inheritdoc />
    protected override IEmitElementSaver CreateNew(PairTypeKey key)
    {
        var collectionType = key.LeftType;
        var elementType = key.RightType;
        // 不支持数组
        if (collectionType.IsArray)
            return null;
        // 不支持字典
        if(ReflectionHelper.HasGenericType(collectionType, typeof(IDictionary<,>)))
            return null;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var isInterface = collectionType.GetTypeInfo().IsInterface;
#else
        var isInterface = collectionType.IsInterface;
#endif
        if (isInterface)
        {
            if (ReflectionHelper.HasGenericType(collectionType, typeof(ICollection<>)))
                return ToCollection(elementType);
            if (ReflectionHelper.HasGenericType(collectionType, typeof(IProducerConsumerCollection<>)))
                return ToTryAdd(typeof(IProducerConsumerCollection<>).MakeGenericType(elementType), elementType);
            return null;
        }
        if (ReflectionHelper.IsGenericType(collectionType, typeof(List<>)))
            return ToAdd(collectionType, elementType);
        if (ReflectionHelper.IsGenericType(collectionType, typeof(HashSet<>)))
            return ToAdd(collectionType, elementType);
        if (ReflectionHelper.IsGenericType(collectionType, typeof(Queue<>)))
            return ToEnqueue(collectionType, elementType);
        if (ReflectionHelper.IsGenericType(collectionType, typeof(Stack<>)))
            return ToPush(collectionType, elementType);
        if (ReflectionHelper.IsGenericType(collectionType, typeof(BlockingCollection<>)))
            return ToAdd(collectionType, elementType);
        if (ReflectionHelper.IsGenericType(collectionType, typeof(ConcurrentQueue<>)))
            return ToEnqueue(collectionType, elementType);
        if (ReflectionHelper.IsGenericType(collectionType, typeof(ConcurrentStack<>)))
            return ToPush(collectionType, elementType);
        if (ReflectionHelper.IsGenericType(collectionType, typeof(ConcurrentBag<>)))
            return ToAdd(collectionType, elementType);
        return ToAdd(collectionType, elementType);
    }

    /// <summary>
    /// 保存到ICollection
    /// </summary>
    /// <param name="elementType"></param>
    /// <returns></returns>
    public static EmitElementSaver ToCollection(Type elementType)
        => new(elementType);
    /// <summary>
    /// Add方法保存
    /// </summary>
    /// <param name="collectionType"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    public static EmitElementSaver ToAdd(Type collectionType, Type elementType)
        => new(collectionType, elementType, "Add");
    /// <summary>
    /// TryAdd方法保存
    /// </summary>
    /// <param name="collectionType"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    public static EmitElementSaver ToTryAdd(Type collectionType, Type elementType)
        => new(collectionType, elementType, "TryAdd");
    /// <summary>
    /// Enqueue方法保存
    /// </summary>
    /// <param name="collectionType"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    public static EmitElementSaver ToEnqueue(Type collectionType, Type elementType)
        => new(collectionType, elementType, "Enqueue");
    /// <summary>
    /// Push方法保存
    /// </summary>
    /// <param name="collectionType"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    public static EmitElementSaver ToPush(Type collectionType, Type elementType)
        => new(collectionType, elementType, "Push");
}