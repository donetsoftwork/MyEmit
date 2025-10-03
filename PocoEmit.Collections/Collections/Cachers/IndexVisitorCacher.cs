using PocoEmit.Collections.Bundles;
using PocoEmit.Collections.Visitors;
using PocoEmit.Dictionaries;
using System;

namespace PocoEmit.Collections.Cachers;

/// <summary>
/// 集合访问者缓存
/// </summary>
/// <param name="container"></param>
internal class IndexVisitorCacher(CollectionContainer container)
    : CacheBase<Type, IElementIndexVisitor>(container)
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
    protected override IElementIndexVisitor CreateNew(in Type key)
        => CreateByType(_container, key);
    /// <summary>
    /// 按类型构造集合访问者
    /// </summary>
    /// <param name="container"></param>
    /// <param name="collectionType"></param>
    /// <returns></returns>
    public static IElementIndexVisitor CreateByType(CollectionContainer container, Type collectionType)
    {
        if (collectionType.IsArray)
            return new ArrayVisitor(collectionType);
        if (container.DictionaryCacher.Validate(collectionType, out var dictionaryBundle))
            return CreateByDictionary(collectionType, dictionaryBundle);
        if (container.ListCacher.Container.ListCacher.Validate(collectionType, out var listBundle))
            return CreateByList(collectionType, listBundle);
        return null;
    }
    /// <summary>
    /// 遍历列表
    /// </summary>
    /// <param name="listType"></param>
    /// <param name="bundle"></param>
    /// <returns></returns>
    private static ListVisitor CreateByList(Type listType, ListBundle bundle)
        => new(listType, bundle.ElementType, bundle.Count, bundle.Items);
    /// <summary>
    /// 获取遍历列表
    /// </summary>
    /// <param name="listType"></param>
    /// <param name="bundle"></param>
    /// <returns></returns>
    public IElementIndexVisitor GetByList(Type listType, ListBundle bundle)
    {
        if (TryGetCache(listType, out IElementIndexVisitor visitor))
            return visitor;
        Set(listType, visitor = CreateByList(listType, bundle));
        return visitor;
    }
    /// <summary>
    /// 遍历字典
    /// </summary>
    /// <param name="dictionaryType"></param>
    /// <param name="bundle"></param>
    /// <returns></returns>
    private static DictionaryIndexVisitor CreateByDictionary(Type dictionaryType, DictionaryBundle bundle)
        => new(dictionaryType, bundle.KeyType, bundle.ValueType, bundle.Keys, bundle.Items);
    /// <summary>
    /// 获取遍历字典
    /// </summary>
    /// <param name="dictionaryType"></param>
    /// <param name="bundle"></param>
    /// <returns></returns>
    public IElementIndexVisitor GetByDictionary(Type dictionaryType, DictionaryBundle bundle)
    {
        if (TryGetCache(dictionaryType, out IElementIndexVisitor visitor))
            return visitor;
        Set(dictionaryType, visitor = CreateByDictionary(dictionaryType, bundle));
        return visitor;
    }
}
