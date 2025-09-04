using PocoEmit.Collections.Bundles;
using PocoEmit.Collections.Indexs;
using PocoEmit.Indexs;
using System;

namespace PocoEmit.Collections.Cachers;

/// <summary>
/// 索引器缓存
/// </summary>
internal class ReadIndexCacher(CollectionContainer container)
    : CacheBase<Type, IEmitIndexMemberReader>(container)
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
    protected override IEmitIndexMemberReader CreateNew(Type key)
    {
        if (key.IsArray)
            return ArrayMemberIndex.Instance;
        if (_container.ListCacher.Validate(key, out var bundle))
            return CreateByList(bundle);
        return null;
    }
    /// <summary>
    /// 构造列表索引读取器
    /// </summary>
    /// <param name="bundle"></param>
    /// <returns></returns>
    private static PropertyIndexMemberReader CreateByList(ListBundle bundle)
        => bundle == null ? null : new(bundle.Items);
    /// <summary>
    /// 获取列表索引读取器
    /// </summary>
    /// <param name="listType"></param>
    /// <param name="bundle"></param>
    /// <returns></returns>
    public IEmitIndexMemberReader GetByList(Type listType, ListBundle bundle)
    {
        if (TryGetValue(listType, out IEmitIndexMemberReader reader))
            return reader;
        Set(listType, reader = CreateByList(bundle));
        return reader;
    }
}
