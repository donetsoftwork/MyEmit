using PocoEmit.Collections.Bundles;
using PocoEmit.Collections.Counters;
using PocoEmit.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PocoEmit.Collections.Cachers;

/// <summary>
/// 获取集合数量缓存
/// </summary>
/// <param name="container"></param>
internal class CountCacher(CollectionContainer container)
    : CacheBase<PairTypeKey, IEmitElementCounter>(container)
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
    protected override IEmitElementCounter CreateNew(in PairTypeKey key)
        => CrateByType(_container, key.LeftType, key.RightType);
    /// <summary>
    /// 按类型获取集合数量
    /// </summary>
    /// <param name="container"></param>
    /// <param name="collectionType"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    public static IEmitElementCounter CrateByType(CollectionContainer container, Type collectionType, Type elementType)
    {
        if (collectionType.IsArray)
            return CreateByArray(collectionType, elementType);
        PropertyInfo countProperty = null;
        if (container.DictionaryCacher.Validate(collectionType, out var dictionaryBundle))
            countProperty = GetCountByDictionary(container, collectionType);
        else if (container.CollectionCacher.Validate(collectionType, out var collectionBundle))
            countProperty = GetCountByCollection(collectionBundle);
        countProperty ??= CollectionContainer.GetCountProperty(collectionType);
        if (countProperty == null)
        {
            if (container.EnumerableCacher.Validate(collectionType))
                return CreateByEnumerable(elementType);
        }
        else
        {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
            var declaringType = countProperty.DeclaringType;
#else
            var declaringType = countProperty.ReflectedType;
#endif
            return new PropertyCounter(declaringType, elementType, countProperty);
        }
        return null;
    }
    /// <summary>
    /// 获取字典数量
    /// </summary>
    /// <param name="container"></param>
    /// <param name="dictionaryType"></param>
    /// <returns></returns>
    public static PropertyInfo GetCountByDictionary(CollectionContainer container, Type dictionaryType)
    {
        var bundle = container.DictionaryCacher.Get(dictionaryType);
        if (bundle is null)
            return null;
        return bundle.Count;
    }
    /// <summary>
    /// 获取字典数量
    /// </summary>
    /// <param name="dictionaryType"></param>
    /// <param name="bundle"></param>
    /// <returns></returns>
    public IEmitElementCounter GetByDictionary(Type dictionaryType, DictionaryBundle bundle)
    {
        var valueType = bundle.ValueType;
        var key = new PairTypeKey(dictionaryType, valueType);
        if(TryGetCache(key, out IEmitElementCounter counter))
            return counter;
        Set(key, counter = new PropertyCounter(dictionaryType, valueType, bundle.Count));
        return counter;
    }
    /// <summary>
    /// 获取集合数量
    /// </summary>
    /// <param name="bundle"></param>
    /// <returns></returns>
    private static PropertyInfo GetCountByCollection(CollectionBundle bundle)
    {
        if (bundle is null)
            return null;
        return bundle.Count;
    }
    /// <summary>
    /// 获取集合数量
    /// </summary>
    /// <param name="collectionType"></param>
    /// <param name="bundle"></param>
    /// <returns></returns>
    public IEmitElementCounter GetByCollection(Type collectionType, CollectionBundle bundle)
    {
        var elementType = bundle.ElementType;
        var key = new PairTypeKey(collectionType, elementType);
        if (TryGetCache(key, out IEmitElementCounter counter))
            return counter;
        Set(key, counter = new PropertyCounter(collectionType, elementType, bundle.Count));
        return counter;
    }
    /// <summary>
    /// 获取迭代数量
    /// </summary>
    /// <param name="elementType"></param>
    /// <returns></returns>
    private static EnumerableCounter CreateByEnumerable(Type elementType)
        => new(typeof(IEnumerable<>).MakeGenericType(elementType), elementType);
    /// <summary>
    /// 获取迭代数量
    /// </summary>
    /// <param name="enumerable"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    public IEmitElementCounter GetByEnumerable(Type enumerable, Type elementType)
    {
        var key = new PairTypeKey(enumerable, elementType);
        if (TryGetCache(key, out IEmitElementCounter counter))
            return counter;
        Set(key, counter = CreateByEnumerable(elementType));
        return counter;
    }
    /// <summary>
    /// 构造获取数组长度
    /// </summary>
    /// <param name="arrayType"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    private static ArrayCounter CreateByArray(Type arrayType, Type elementType)
        => arrayType.GetArrayRank() == 1 ? new(arrayType, elementType) : null;
    /// <summary>
    /// 获取数组长度
    /// </summary>
    /// <param name="arrayType"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    public IEmitElementCounter GetByArray(Type arrayType, Type elementType)
    {
        var key = new PairTypeKey(arrayType, elementType);
        if (TryGetCache(key, out IEmitElementCounter counter))
            return counter;
        Set(key, counter = CreateByArray(arrayType, elementType));
        return counter;
    }
}
