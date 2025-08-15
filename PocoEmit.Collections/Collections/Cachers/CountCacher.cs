using PocoEmit.Collections.Counters;
using PocoEmit.Configuration;
using PocoEmit.Dictionaries;
using System;
using System.Collections.Generic;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
using System.Reflection;
#endif

namespace PocoEmit.Collections.Cachers;

/// <summary>
/// 获取集合数量缓存
/// </summary>
/// <param name="cacher"></param>
internal class CountCacher(ICacher<PairTypeKey, IEmitElementCounter> cacher)
    : CacheBase<PairTypeKey, IEmitElementCounter>(cacher)
{
    /// <inheritdoc />
    protected override IEmitElementCounter CreateNew(PairTypeKey key)
    {
        var collectionType = key.LeftType;
        var elementType = key.RightType;
        if (collectionType.IsArray)
        {
            if(collectionType.GetArrayRank() == 1)
                return new ArrayCounter(collectionType, elementType);
        }
        var counter = CreateByProperty(collectionType, elementType);
        if(counter == null)
        {
            if (ReflectionHelper.HasGenericType(collectionType, typeof(IDictionary<,>)))
                return CreateByDictionary(collectionType, elementType);
            if (ReflectionHelper.HasGenericType(collectionType, typeof(ICollection<>)))
                return CreateByProperty(typeof(ICollection<>).MakeGenericType(elementType), elementType);
            else if (ReflectionHelper.HasGenericType(collectionType, typeof(IEnumerable<>)))
                return new EnumerableCounter(typeof(IEnumerable<>).MakeGenericType(elementType), elementType);
        }
        return counter;
    }
    /// <summary>
    /// 按属性获取数量
    /// </summary>
    /// <param name="collectionType"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    private static PropertyCounter CreateByProperty(Type collectionType, Type elementType)
    {
        var countProperty = PropertyCounter.GetCountProperty(collectionType);
        if(countProperty is null)
            return null;
        return new PropertyCounter(collectionType, elementType, countProperty);
    }
    /// <summary>
    /// 获取字典数量
    /// </summary>
    /// <param name="dictionaryType"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    private static PropertyCounter CreateByDictionary(Type dictionaryType, Type elementType)
    {
        var keys = EmitDictionaryBase.GetKeysProperty(dictionaryType);
        if (keys is null)
            return null;
        var keyType = ReflectionHelper.GetElementType(keys.PropertyType);
        if (keyType is null)
            return null;
        var collectionType = typeof(ICollection<>).MakeGenericType(EmitDictionaryBase.MakePairType(keyType, elementType));
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        if (collectionType.GetTypeInfo().IsAssignableFrom(dictionaryType.GetTypeInfo()))
#else
        if (collectionType.IsAssignableFrom(dictionaryType))
#endif
            return CreateByProperty(collectionType, elementType);
        return null;
    }
}
