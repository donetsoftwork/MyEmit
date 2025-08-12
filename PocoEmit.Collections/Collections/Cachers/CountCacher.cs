using PocoEmit.Collections.Counters;
using PocoEmit.Dictionaries;
using System;
using System.Collections.Generic;

namespace PocoEmit.Collections.Cachers;

/// <summary>
/// 获取集合数量缓存
/// </summary>
/// <param name="cacher"></param>
internal class CountCacher(ICacher<Type, IEmitCollectionCounter> cacher)
    : CacheBase<Type, IEmitCollectionCounter>(cacher)
{
    /// <inheritdoc />
    protected override IEmitCollectionCounter CreateNew(Type key)
    {
        if (key.IsArray)
            return new ArrayCounter(key);
        if(ReflectionHelper.HasGenericType(key, typeof(IDictionary<,>)))
            return CreateByDictionary(key);
        var elementType = ReflectionHelper.GetElementType(key);
        if (elementType == null)
            return null;
        var countProperty = PropertyCounter.GetCountProperty(key);
        if (countProperty is not null)
            return new PropertyCounter(key, elementType, countProperty);
        if (ReflectionHelper.HasGenericType(key, typeof(ICollection<>)))
            return Get(typeof(ICollection<>).MakeGenericType(elementType));
        else if (ReflectionHelper.HasGenericType(key, typeof(IEnumerable<>)))
            return new EnumerableCounter(key, elementType);
        return null;
    }
    /// <summary>
    /// 获取字典数量
    /// </summary>
    /// <param name="dctionaryType"></param>
    /// <returns></returns>
    private IEmitCollectionCounter CreateByDictionary(Type dctionaryType)
    {
        var arguments = ReflectionHelper.GetGenericArguments(dctionaryType);
        if(arguments.Length != 2)
            return null;
        var countProperty = PropertyCounter.GetCountProperty(dctionaryType);
        if (countProperty is not null)
            return new PropertyCounter(dctionaryType, arguments[1], countProperty);
        var collectionType = typeof(ICollection<>).MakeGenericType(EmitDictionaryBase.MakePairType(arguments));
        return Get(collectionType);
    }
}
