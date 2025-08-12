using PocoEmit.Collections.Visitors;
using PocoEmit.Dictionaries;
using System;
using System.Collections.Generic;

namespace PocoEmit.Collections.Cachers;

/// <summary>
/// 集合访问者缓存
/// </summary>
/// <param name="cacher"></param>
internal class VisitorCacher(ICacher<Type, ICollectionVisitor> cacher)
    : CacheBase<Type, ICollectionVisitor>(cacher)
{
    /// <inheritdoc />
    protected override ICollectionVisitor CreateNew(Type key)
    {
        if (key.IsArray)
            return new ArrayVisitor(key);
        if (ReflectionHelper.HasGenericType(key, typeof(IDictionary<,>)))
            return CreateByDictionary(key);
        var elementType = ReflectionHelper.GetElementType(key);
        if (elementType == null)
            return null;
        if (ReflectionHelper.HasGenericType(key, typeof(IList<>)))
            return new ListVisitor(key, elementType);
        else if (ReflectionHelper.HasGenericType(key, typeof(IEnumerable<>)))
            return new EnumerableVisitor(elementType);
        return null;
    }

    /// <summary>
    /// 获取字典数量
    /// </summary>
    /// <param name="dictionaryType"></param>
    /// <returns></returns>
    private static DictionaryValuesVisitor CreateByDictionary(Type dictionaryType)
    {
        var arguments = ReflectionHelper.GetGenericArguments(dictionaryType);
        if (arguments.Length != 2)
            return null;
        return new DictionaryValuesVisitor(dictionaryType, arguments[1]);
    }
}
