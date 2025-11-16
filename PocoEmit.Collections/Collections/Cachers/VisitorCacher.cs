using Hand.Cache;
using PocoEmit.Collections.Bundles;
using PocoEmit.Collections.Visitors;
using PocoEmit.Dictionaries;
using System;

namespace PocoEmit.Collections.Cachers;

/// <summary>
/// 集合访问者缓存
/// </summary>
/// <param name="container"></param>
internal class VisitorCacher(CollectionContainer container)
    : CacheFactoryBase<Type, IEmitElementVisitor>(container)
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
    protected override IEmitElementVisitor CreateNew(in Type key)
        => CreateByType(_container, key);
    /// <summary>
    /// 按类型构造集合访问者
    /// </summary>
    /// <param name="container"></param>
    /// <param name="collectionType"></param>
    /// <returns></returns>
    public static IEmitElementVisitor CreateByType(CollectionContainer container, Type collectionType)
    {
        if (collectionType.IsArray)
            return CreateByArray(collectionType);
        if (container.DictionaryCacher.Validate(collectionType, out var dictionaryBundle))
            return CreateByDictionary(collectionType, dictionaryBundle);
        if (container.ListCacher.Validate(collectionType, out var listBundle))
            return CreateByList(collectionType, listBundle);
        else if (container.EnumerableCacher.Validate(collectionType, out var enumerableBundle))
            return CreateByEnumerable(collectionType, enumerableBundle);
        return null;
    }
    /// <summary>
    /// 构造数组访问者
    /// </summary>
    /// <param name="arrayType"></param>
    /// <returns></returns>
    private static ArrayVisitor CreateByArray(Type arrayType)
        => new(arrayType);
    /// <summary>
    /// 获取数组访问者
    /// </summary>
    /// <param name="arrayType"></param>
    /// <returns></returns>
    public IEmitElementVisitor GetByByArray(Type arrayType)
    {
        if (TryGetCache(arrayType, out IEmitElementVisitor visitor))
            return visitor;
        Save(arrayType, visitor = CreateByArray(arrayType));
        return visitor;
    }
    /// <summary>
    /// 构造字典值访问者
    /// </summary>
    /// <param name="dictionaryType"></param>
    /// <param name="bundle"></param>
    /// <returns></returns>
    private static DictionaryValuesVisitor CreateByDictionary(Type dictionaryType, DictionaryBundle bundle)
    {
        if (bundle is null)
            return null;
        return new(dictionaryType, bundle.ValueType, bundle.Values);
    }
    /// <summary>
    /// 获取字典值访问者
    /// </summary>
    /// <param name="dictionaryType"></param>
    /// <param name="bundle"></param>
    /// <returns></returns>
    public IEmitElementVisitor GetByDictionary(Type dictionaryType, DictionaryBundle bundle)
    {
        if (TryGetCache(dictionaryType, out IEmitElementVisitor visitor))
            return visitor;
        Save(dictionaryType, visitor = CreateByDictionary(dictionaryType, bundle));
        return visitor;
    }
    /// <summary>
    /// 构造列表访问者
    /// </summary>
    /// <param name="listType"></param>
    /// <param name="bundle"></param>
    /// <returns></returns>
    private static ListVisitor CreateByList(Type listType, ListBundle bundle)
    {
        if (bundle is null)
            return null;
        return new(listType, bundle.ElementType, bundle.Count, bundle.Items);
    }
    /// <summary>
    /// 获取列表访问者
    /// </summary>
    /// <param name="listType"></param>
    /// <param name="bundle"></param>
    /// <returns></returns>
    public IEmitElementVisitor GetByList(Type listType, ListBundle bundle)
    {
        if (TryGetCache(listType, out IEmitElementVisitor visitor))
            return visitor;
        Save(listType, visitor = CreateByList(listType, bundle));
        return visitor;
    }
    /// <summary>
    /// 构造迭代访问者
    /// </summary>
    /// <param name="enumerableType"></param>
    /// <param name="bundle"></param>
    /// <returns></returns>
    private static EnumerableVisitor CreateByEnumerable(Type enumerableType, EnumerableBundle bundle)
    {
        if (bundle is null)
            return null;
        return new(enumerableType, bundle.ElementType, bundle.GetEnumeratorMethod, bundle.EnumeratorType, bundle.MoveNextMethod, bundle.CurrentProperty);
    }
    /// <summary>
    /// 获取迭代访问者
    /// </summary>
    /// <param name="enumerableType"></param>
    /// <param name="bundle"></param>
    /// <returns></returns>
    public IEmitElementVisitor GetByEnumerable(Type enumerableType, EnumerableBundle bundle)
    {
        if (TryGetCache(enumerableType, out IEmitElementVisitor visitor))
            return visitor;
        Save(enumerableType, visitor = CreateByEnumerable(enumerableType, bundle));
        return visitor;
    }
}
