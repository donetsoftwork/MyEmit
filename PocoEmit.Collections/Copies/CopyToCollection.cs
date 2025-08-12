using PocoEmit.Collections.Copies;
using PocoEmit.Collections.Visitors;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
using System.Reflection;
#endif

namespace PocoEmit.Copies;

/// <summary>
/// 复制数据到集合
/// </summary>
public class CopyToCollection(IMapperOptions options)
{
    #region 配置
    /// <summary>
    /// Emit配置
    /// </summary>
    protected readonly IMapperOptions _options = options;
    /// <summary>
    /// Emit配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    #endregion
    /// <summary>
    /// 复制数据到集合
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public IEmitCopier ToCollection(MapTypeKey key)
        => Create(key.SourceType, key.DestType, true);
    /// <summary>
    /// 构造复制器
    /// </summary>
    /// <param name="sourcetype"></param>
    /// <param name="destType"></param>
    /// <param name="clear"></param>
    /// <returns></returns>
    public IEmitCopier Create(Type sourcetype, Type destType,  bool clear = true)
    {
        var sourceElementType = ReflectionHelper.GetElementType(sourcetype);
        if (sourceElementType == null)
            return null;
        var destElementType = ReflectionHelper.GetElementType(destType);
        if (destElementType == null)
            return null;
        var elementConverter = _options.GetEmitConverter(sourceElementType, destElementType);
        if (elementConverter is null)
            return null;
        CollectionContainer container = CollectionContainer.Instance;
        var visitor = container.GetVisitor(sourcetype);
        if (visitor is null)
            return null;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var isInterface = destType.GetTypeInfo().IsInterface;
#else
        var isInterface = destType.IsInterface;
#endif
        if (isInterface)
        {
            if (ReflectionHelper.HasGenericType(destType, typeof(ICollection<>)))
                return ToCollection(destElementType, visitor, elementConverter, clear);
            if (ReflectionHelper.HasGenericType(destType, typeof(IProducerConsumerCollection<>)))
                return ToTryAdd(typeof(IProducerConsumerCollection<>).MakeGenericType(destElementType), destElementType, visitor, elementConverter, clear);
            return null;
        }
        if (ReflectionHelper.IsGenericType(destType, typeof(List<>)))
            return ToAdd(destType, destElementType, visitor, elementConverter, clear);
        if (ReflectionHelper.IsGenericType(destType, typeof(HashSet<>)))
            return ToAdd(destType, destElementType, visitor, elementConverter, clear);
        if (ReflectionHelper.IsGenericType(destType, typeof(Queue<>)))
            return ToEnqueue(destType, destElementType, visitor, elementConverter, clear);
        if (ReflectionHelper.IsGenericType(destType, typeof(Stack<>)))
            return ToPush(destType, destElementType, visitor, elementConverter, clear);
        if (ReflectionHelper.IsGenericType(destType, typeof(BlockingCollection<>)))
            return ToAdd(destType, destElementType, visitor, elementConverter, clear);
        if (ReflectionHelper.IsGenericType(destType, typeof(ConcurrentQueue<>)))
            return ToEnqueue(destType, destElementType, visitor, elementConverter, clear);
        if (ReflectionHelper.IsGenericType(destType, typeof(ConcurrentStack<>)))
            return ToPush(destType, destElementType, visitor, elementConverter, clear);
        if (ReflectionHelper.IsGenericType(destType, typeof(ConcurrentBag<>)))
            return ToAdd(destType, destElementType, visitor, elementConverter, clear);
        return ToAdd(destType, destElementType, visitor, elementConverter, clear);
    }
    /// <summary>
    /// 复制到ICollection
    /// </summary>
    /// <param name="elementType"></param>
    /// <param name="sourceVisitor"></param>
    /// <param name="elementConverter"></param>
    /// <param name="clear"></param>
    /// <returns></returns>
    public static CollectionCopier ToCollection(Type elementType, ICollectionVisitor sourceVisitor, IEmitConverter elementConverter, bool clear = true)
        => new(elementType, sourceVisitor, elementConverter, clear);
    /// <summary>
    /// Add方法复制
    /// </summary>
    /// <param name="destType"></param>
    /// <param name="elementType"></param>
    /// <param name="sourceVisitor"></param>
    /// <param name="elementConverter"></param>
    /// <param name="clear"></param>
    /// <returns></returns>
    public static CollectionCopier ToAdd(Type destType, Type elementType, ICollectionVisitor sourceVisitor, IEmitConverter elementConverter, bool clear = true)
        => new(destType, elementType, "Add", sourceVisitor, elementConverter, clear);
    /// <summary>
    /// TryAdd方法复制
    /// </summary>
    /// <param name="destType"></param>
    /// <param name="elementType"></param>
    /// <param name="sourceVisitor"></param>
    /// <param name="elementConverter"></param>
    /// <param name="clear"></param>
    /// <returns></returns>
    public static CollectionCopier ToTryAdd(Type destType, Type elementType, ICollectionVisitor sourceVisitor, IEmitConverter elementConverter, bool clear = true)
        => new(destType, elementType, "TryAdd", sourceVisitor, elementConverter, clear);
    /// <summary>
    /// Enqueue方法复制
    /// </summary>
    /// <param name="destType"></param>
    /// <param name="elementType"></param>
    /// <param name="sourceVisitor"></param>
    /// <param name="elementConverter"></param>
    /// <param name="clear"></param>
    /// <returns></returns>
    public static CollectionCopier ToEnqueue(Type destType, Type elementType, ICollectionVisitor sourceVisitor, IEmitConverter elementConverter, bool clear = true)
        => new(destType, elementType, "Enqueue", sourceVisitor, elementConverter, clear);
    /// <summary>
    /// Push方法复制
    /// </summary>
    /// <param name="destType"></param>
    /// <param name="elementType"></param>
    /// <param name="sourceVisitor"></param>
    /// <param name="elementConverter"></param>
    /// <param name="clear"></param>
    /// <returns></returns>
    public static CollectionCopier ToPush(Type destType, Type elementType, ICollectionVisitor sourceVisitor, IEmitConverter elementConverter, bool clear = true)
        => new(destType, elementType, "Push", sourceVisitor, elementConverter, clear);
}
