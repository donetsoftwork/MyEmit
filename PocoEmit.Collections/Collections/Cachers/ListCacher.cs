using PocoEmit.Collections.Bundles;
using System;
using System.Collections.Generic;

namespace PocoEmit.Collections.Cachers;

/// <summary>
/// 列表成员缓存
/// </summary>
/// <param name="container"></param>
internal class ListCacher(CollectionContainer container)
    : CacheBase<Type, ListBundle>(container)
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
    protected override ListBundle CreateNew(in Type key)
    {
        if (!ReflectionHelper.HasGenericType(key, typeof(IList<>)))
            return null;
        return CreateByType(_container, key);
    }
    /// <summary>
    /// 验证列表类型是否合法
    /// </summary>
    /// <param name="listType"></param>
    /// <returns></returns>
    public bool Validate(Type listType)
        => Validate(listType, out var _);
    /// <summary>
    /// 验证列表类型是否合法
    /// </summary>
    /// <param name="listType"></param>
    /// <param name="bundle"></param>
    /// <returns></returns>
    public bool Validate(Type listType, out ListBundle bundle)
    {
        if (ReflectionHelper.HasGenericType(listType, typeof(IList<>)))
            return (bundle = Get(listType)) is not null;
        return TryGetValue(listType, out bundle) && bundle is not null;
    }
    /// <summary>
    /// 获取列表成员
    /// </summary>
    /// <param name="container"></param>
    /// <param name="listType"></param>
    /// <returns></returns>
    public static ListBundle CreateByType(CollectionContainer container, Type listType)
    {
        var collection = container.CollectionCacher.Get(listType);
        if (collection is null)
            return null;
        var items = CollectionContainer.GetItemProperty(listType, typeof(int));
        if (items is null)
            return null;
        return new(items, collection.IsInterface, collection.ElementType, collection.CapacityConstructor, collection.AddMethod, collection.Count);
    }
}
