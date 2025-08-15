using PocoEmit.Collections.Converters;
using PocoEmit.Collections.Counters;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace PocoEmit.Converters;

/// <summary>
/// 转化为集合
/// </summary>
/// <param name="options"></param>
public class ConvertToCollection(IMapperOptions options)
{
    #region 配置
    private readonly IMapperOptions _options = options;
    /// <summary>
    /// 配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    #endregion
    /// <summary>
    /// 转化为集合
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="isInterface"></param>
    /// <returns></returns>
    public IEmitConverter ToCollection(Type sourceType, Type destType, bool isInterface)
    {
        var sourceElementType = ReflectionHelper.GetElementType(sourceType);
        if (sourceElementType == null)
            return null;
        var destElementType = ReflectionHelper.GetElementType(destType);
        if (destType == null)
            return null;
        IEmitCounter sourceCount = CollectionContainer.Instance.CountCacher.Get(sourceType, sourceElementType);
        if (isInterface)
        {
            if (ReflectionHelper.HasGenericType(destType, typeof(IList<>)))
                return ToList(sourceType, destElementType, sourceCount);
            if (ReflectionHelper.HasGenericType(destType, typeof(ISet<>)))
                return ToHashSet(sourceType, destElementType, sourceCount);
            if (ReflectionHelper.HasGenericType(destType, typeof(ICollection<>)))
                return ToList(sourceType, destElementType, sourceCount);
            if (ReflectionHelper.HasGenericType(destType, typeof(IProducerConsumerCollection<>)))
                return ToConcurrentBag(sourceType, destElementType, sourceCount);
            if (ReflectionHelper.HasGenericType(destType, typeof(IEnumerable<>)))
                return ToList(sourceType, destElementType, sourceCount);
            return null;
        }
        IEmitCopier copier = _options.GetCollectionCopier().CollectionCopier.Create(sourceType, destType, false);
        if (copier is null)
            return null;
        
        return new CollectionConverter(destType, destElementType, sourceCount, copier);
    }
    /// <summary>
    /// 转化为列表
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destElementType"></param>
    /// <param name="sourceCount"></param>
    /// <returns></returns>
    public CollectionConverter ToList(Type sourceType, Type destElementType, IEmitCounter sourceCount)
    {
        var collectionType = typeof(List<>).MakeGenericType(destElementType);
        IEmitCopier copier = _options.GetCollectionCopier().CollectionCopier.Create(sourceType, collectionType, false);
        return new(collectionType, destElementType, sourceCount, copier);
    }
    /// <summary>
    /// 转化为HashSet
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destElementType"></param>
    /// <param name="sourceCount"></param>
    /// <returns></returns>
    public CollectionConverter ToHashSet(Type sourceType, Type destElementType, IEmitCounter sourceCount)
    {
        var collectionType = typeof(HashSet<>).MakeGenericType(destElementType);
        IEmitCopier copier = _options.GetCollectionCopier().CollectionCopier.Create(sourceType, collectionType, false);
        return new(collectionType, destElementType, sourceCount, copier);
    }
    /// <summary>
    /// 转化为ConcurrentBag
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destElementType"></param>
    /// <param name="sourceCount"></param>
    /// <returns></returns>
    public CollectionConverter ToConcurrentBag(Type sourceType, Type destElementType, IEmitCounter sourceCount)
    {
        var collectionType = typeof(ConcurrentBag<>).MakeGenericType(destElementType);
        IEmitCopier copier = _options.GetCollectionCopier().CollectionCopier.Create(sourceType, collectionType, false);
        return new(collectionType, destElementType, sourceCount, copier);
    }
}
