using PocoEmit.Collections.Bundles;
using PocoEmit.Collections.Converters;
using PocoEmit.Collections.Counters;
using PocoEmit.Complexes;
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
    /// <param name="destBundle"></param>
    /// <returns></returns>
    public IComplexIncludeConverter ToCollection(Type sourceType, Type destType, CollectionBundle destBundle)
    {
        var container = CollectionContainer.Instance;
        if (sourceType.IsArray)
            return ArrayToCollection(sourceType, ReflectionHelper.GetElementType(sourceType), destType, destBundle);
        if (container.DictionaryCacher.Validate(sourceType, out var dictionaryBundle))
            return DictionaryToCollection(sourceType, dictionaryBundle, destType, destBundle);
        if (container.ListCacher.Validate(sourceType, out var listBundle))
            return ListToCollection(sourceType, listBundle, destType, destBundle);
        if (container.EnumerableCacher.Validate(sourceType, out var enumerableBundle))
            return EnumerableToCollection(sourceType, enumerableBundle, destType, destBundle);
        return ElementToCollection(sourceType, destType, destBundle);
    }
    /// <summary>
    /// 数组转化为集合
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="sourceElementType"></param>
    /// <param name="destType"></param>
    /// <param name="destBundle"></param>
    /// <returns></returns>
    private CollectionConverter ArrayToCollection(Type sourceType, Type sourceElementType, Type destType, CollectionBundle destBundle)
    {
        IEmitCopier copier = _options.GetCollectionCopier().CollectionCopier
            .ArrayToCollection(sourceType, sourceElementType, destType, destBundle, false);
        if (copier is null)
            return null;
        IEmitCounter sourceCount = null;
        var capacityConstructor = destBundle.CapacityConstructor;
        // 容量构造函数存在才需要获取sourceCount
        if (capacityConstructor is not null)
            sourceCount = CollectionContainer.Instance.CountCacher.GetByArray(sourceType, sourceElementType);
        return new CollectionConverter(destType, destBundle.ElementType, capacityConstructor, sourceCount, copier);
    }
    /// <summary>
    /// 字典转集合
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="sourceBundle"></param>
    /// <param name="destType"></param>
    /// <param name="destBundle"></param>
    /// <returns></returns>
    private CollectionConverter DictionaryToCollection(Type sourceType, DictionaryBundle sourceBundle, Type destType, CollectionBundle destBundle)
    {
        IEmitCopier copier = _options.GetCollectionCopier().CollectionCopier
            .DictionaryToCollection(sourceType, sourceBundle, destType, destBundle, false);
        if (copier is null)
            return null;
        IEmitCounter sourceCount = null;
        var capacityConstructor = destBundle.CapacityConstructor;
        // 容量构造函数存在才需要获取sourceCount
        if (capacityConstructor is not null)
            sourceCount = CollectionContainer.Instance.CountCacher.GetByDictionary(sourceType, sourceBundle);
        return new CollectionConverter(destType, destBundle.ElementType, capacityConstructor, sourceCount, copier);
    }
    /// <summary>
    /// 列表转集合
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="sourceBundle"></param>
    /// <param name="destType"></param>
    /// <param name="destBundle"></param>
    /// <returns></returns>
    private CollectionConverter ListToCollection(Type sourceType, ListBundle sourceBundle, Type destType, CollectionBundle destBundle)
    {
        IEmitCopier copier = _options.GetCollectionCopier().CollectionCopier
            .ListToCollection(sourceType, sourceBundle, destType, destBundle, false);
        if (copier is null)
            return null;
        IEmitCounter sourceCount = null;
        var capacityConstructor = destBundle.CapacityConstructor;
        // 容量构造函数存在才需要获取sourceCount
        if (capacityConstructor is not null)
            sourceCount = CollectionContainer.Instance.CountCacher.GetByCollection(sourceType, sourceBundle);
        return new CollectionConverter(destType, destBundle.ElementType, capacityConstructor, sourceCount, copier);
    }
    /// <summary>
    /// 迭代转集合
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="sourceBundle"></param>
    /// <param name="destType"></param>
    /// <param name="destBundle"></param>
    /// <returns></returns>
    private CollectionConverter EnumerableToCollection(Type sourceType, EnumerableBundle sourceBundle, Type destType, CollectionBundle destBundle)
    {
        IEmitCopier copier = _options.GetCollectionCopier().CollectionCopier
            .EnumerableToCollection(sourceType, sourceBundle, destType, destBundle, false);
        if (copier is null)
            return null;
        IEmitCounter sourceCount = null;
        var capacityConstructor = destBundle.CapacityConstructor;
        // 容量构造函数存在才需要获取sourceCount
        if (capacityConstructor is not null)
            sourceCount = CollectionContainer.Instance.CountCacher.GetByEnumerable(sourceType, sourceBundle.ElementType);
        return new CollectionConverter(destType, destBundle.ElementType, capacityConstructor, sourceCount, copier);
    }
    ///// <summary>
    ///// 其他情况
    ///// </summary>
    ///// <param name="sourceType"></param>
    ///// <param name="collectionType"></param>
    ///// <param name="elementType"></param>
    ///// <param name="isObject"></param>
    ///// <returns></returns>
    //public IEmitConverter OthersToCollection(Type sourceType, Type collectionType, Type elementType, bool isObject)
    //{
    //    var bundle = _options.MemberCacher.Get(sourceType).EmitReaders;
    //    if (bundle.Count == 0)
    //    {
    //        if (isObject)
    //            return ElementToCollection(sourceType, collectionType, elementType);
    //        return null;
    //    }
    //    var saver = CollectionContainer.Instance.SaveCacher.Get(collectionType, elementType);
    //    if (saver is null)
    //        return null;
    //    if (isObject)
    //        return new MemberCollectionConverter(_options, collectionType, elementType, saver, bundle, bundle.Keys);
    //    var members = bundle.Values
    //        .Where(m => PairTypeKey.CheckValueType(m.ValueType, elementType))
    //        .Select(m => m.Name)
    //        .ToList();
    //    if (members.Count == 0)
    //        return ElementToCollection(sourceType, collectionType, elementType);
    //    return new MemberCollectionConverter(_options, collectionType, elementType, saver, bundle, members);
    //}
    /// <summary>
    /// 子元素转集合
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="collectionType"></param>
    /// <param name="destBundle"></param>
    /// <returns></returns>
    public CollectionInitConverter ElementToCollection(Type sourceType, Type collectionType, CollectionBundle destBundle)
    {
        Type elementType = destBundle.ElementType;
        if (!PairTypeKey.CheckValueType(sourceType, elementType))
            return null;
        var elementConverter = _options.GetEmitConverter(sourceType, elementType);
        if (elementConverter is null)
            return null;
        var saver = CollectionContainer.Instance.SaveCacher.GetByCollection(collectionType, destBundle);
        if (saver is null)
            return null;
        return new CollectionInitConverter(collectionType, elementType, saver, elementConverter);
    }
    /// <summary>
    /// 接口转化为实现类型
    /// </summary>
    /// <param name="interface"></param>
    /// <returns></returns>
    public static Type CheckGenericImplType(Type @interface)
    {
        if (ReflectionHelper.HasGenericType(@interface, typeof(IList<>)))
            return typeof(List<>);
        if (ReflectionHelper.HasGenericType(@interface, typeof(ISet<>)))
            return typeof(HashSet<>);
        if (ReflectionHelper.HasGenericType(@interface, typeof(ICollection<>)))
            return typeof(List<>);
        if (ReflectionHelper.HasGenericType(@interface, typeof(IProducerConsumerCollection<>)))
            return typeof(ConcurrentBag<>);
        if (ReflectionHelper.HasGenericType(@interface, typeof(IEnumerable<>)))
            return typeof(List<>);
        return null;
    }    
}
