using PocoEmit.Collections;
using PocoEmit.Collections.Cachers;
using PocoEmit.Collections.Counters;
using PocoEmit.Collections.Visitors;
using PocoEmit.Configuration;
using PocoEmit.Indexs;
using System;
using System.Collections.Concurrent;
using PocoEmit.Collections.Saves;
using PocoEmit.Collections.Bundles;

namespace PocoEmit;

/// <summary>
/// 集合容器
/// </summary>
public sealed partial class CollectionContainer
    : ICacher<PairTypeKey, IEmitElementCounter>
    , ICacher<Type, IEmitElementVisitor>
    , ICacher<Type, IEmitIndexMemberReader>
    , ICacher<Type, IElementIndexVisitor>
    , ICacher<PairTypeKey, IEmitElementSaver>
    , ICacher<Type, EnumerableBundle>
    , ICacher<Type, CollectionBundle>
    , ICacher<Type, ListBundle>
    , ICacher<Type, DictionaryBundle>
{
    /// <summary>
    /// 集合容器
    /// </summary>
    private CollectionContainer(CollectionContainerOptions options)
    {
        var concurrencyLevel = options.ConcurrencyLevel;
        _counters = new ConcurrentDictionary<PairTypeKey, IEmitElementCounter>(concurrencyLevel, options.CounterCapacity);
        _visitors = new ConcurrentDictionary<Type, IEmitElementVisitor>(concurrencyLevel, options.VisitorCapacity);
        _readIndexs = new ConcurrentDictionary<Type, IEmitIndexMemberReader>(concurrencyLevel, options.ReadIndexCapacity);
        _indexVisitors = new ConcurrentDictionary<Type, IElementIndexVisitor>(concurrencyLevel, options.ReadIndexCapacity);
        _savers = new ConcurrentDictionary<PairTypeKey, IEmitElementSaver>(concurrencyLevel, options.SaverCapacity);
        _enumerables = new ConcurrentDictionary<Type, EnumerableBundle>(concurrencyLevel, options.EnumerableCapacity);
        _collections = new ConcurrentDictionary<Type, CollectionBundle>(concurrencyLevel, options.Collectionapacity);
        _lists = new ConcurrentDictionary<Type, ListBundle>(concurrencyLevel, options.ListCapacity);
        _dictionaries = new ConcurrentDictionary<Type, DictionaryBundle>(concurrencyLevel, options.DictionaryCapacity);

        _countCacher = new(this);
        _visitorCacher = new(this);
        _readIndexCacher = new(this);
        _indexVisitorCacher = new(this);
        _saveCacher = new(this);
        _enumerableCacher = new(this);
        _collectionCacher = new(this);
        _listCacher = new(this);
        _dictionaryCacher = new(this);
    }
    #region 配置
    private readonly CountCacher _countCacher;
    private readonly VisitorCacher _visitorCacher;
    private readonly ReadIndexCacher _readIndexCacher;
    private readonly IndexVisitorCacher _indexVisitorCacher;
    private readonly SaveCacher _saveCacher;
    private readonly EnumerableCacher _enumerableCacher;
    private readonly CollectionCacher _collectionCacher;
    private readonly ListCacher _listCacher;
    private readonly DictionaryCacher _dictionaryCacher;

    /// <summary>
    /// 集合数量缓存
    /// </summary>
    private readonly ConcurrentDictionary<PairTypeKey, IEmitElementCounter> _counters;
    /// <summary>
    /// 集合访问者缓存
    /// </summary>
    private readonly ConcurrentDictionary<Type, IEmitElementVisitor> _visitors;
    /// <summary>
    /// 索引读取器缓存
    /// </summary>
    private readonly ConcurrentDictionary<Type, IEmitIndexMemberReader> _readIndexs;
        /// <summary>
    /// 索引访问者缓存(主要用于字典遍历)
    /// </summary>
    private readonly ConcurrentDictionary<Type, IElementIndexVisitor> _indexVisitors;
        /// <summary>
    /// 元素保存器
    /// </summary>
    private readonly ConcurrentDictionary<PairTypeKey, IEmitElementSaver> _savers;
    /// <summary>
    /// 迭代器成员
    /// </summary>
    private readonly ConcurrentDictionary<Type, EnumerableBundle> _enumerables;
    /// <summary>
    /// 集合类成员
    /// </summary>
    private readonly ConcurrentDictionary<Type, CollectionBundle> _collections;
    /// <summary>
    /// 列表类成员
    /// </summary>
    private readonly ConcurrentDictionary<Type, ListBundle> _lists;
    /// <summary>
    /// 字典类成员
    /// </summary>
    private readonly ConcurrentDictionary<Type, DictionaryBundle> _dictionaries;
    #endregion
    #region ICacher<Type, IEmitElementCounter>
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IEmitElementCounter>.ContainsKey(in PairTypeKey key)
        => _counters.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IEmitElementCounter>.TryGetCache(in PairTypeKey key, out IEmitElementCounter cached)
        => _counters.TryGetValue(key, out cached);
    /// <inheritdoc />
    void IStore<PairTypeKey, IEmitElementCounter>.Set(in PairTypeKey key, IEmitElementCounter value)
        => _counters[key] = value;
    #endregion
    #region ICacher<Type, ICollectionVisitor>
    /// <inheritdoc />
    bool ICacher<Type, IEmitElementVisitor>.ContainsKey(in Type key)
        => _visitors.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<Type, IEmitElementVisitor>.TryGetCache(in Type key, out IEmitElementVisitor cached)
        => _visitors.TryGetValue(key, out cached);
    /// <inheritdoc />
    void IStore<Type, IEmitElementVisitor>.Set(in Type key, IEmitElementVisitor value)
        => _visitors[key] = value;
    #endregion
    #region ICacher<Type, IEmitIndexMemberReader>
    /// <inheritdoc />
    bool ICacher<Type, IEmitIndexMemberReader>.ContainsKey(in Type key)
        => _readIndexs.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<Type, IEmitIndexMemberReader>.TryGetCache(in Type key, out IEmitIndexMemberReader cached)
        => _readIndexs.TryGetValue(key, out cached);
    /// <inheritdoc />
    void IStore<Type, IEmitIndexMemberReader>.Set(in Type key, IEmitIndexMemberReader value)
        => _readIndexs[key] = value;
    #endregion
    #region ICacher<Type, IElementIndexVisitor>
    /// <inheritdoc />
    bool ICacher<Type, IElementIndexVisitor>.ContainsKey(in Type key)
        => _indexVisitors.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<Type, IElementIndexVisitor>.TryGetCache(in Type key, out IElementIndexVisitor cached)
        => _indexVisitors.TryGetValue(key, out cached);
    /// <inheritdoc />
    void IStore<Type, IElementIndexVisitor>.Set(in Type key, IElementIndexVisitor value)
        => _indexVisitors[key] = value;
    #endregion
    #region ICacher<Type, IEmitElementSaver>
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IEmitElementSaver>.ContainsKey(in PairTypeKey key)
        => _savers.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IEmitElementSaver>.TryGetCache(in PairTypeKey key, out IEmitElementSaver cached)
        => _savers.TryGetValue(key, out cached);
    /// <inheritdoc />
    void IStore<PairTypeKey, IEmitElementSaver>.Set(in PairTypeKey key, IEmitElementSaver value)
        => _savers[key] = value;
    #endregion
    #region ICacher<Type, EnumerableBundle>
    /// <inheritdoc />
    bool ICacher<Type, EnumerableBundle>.ContainsKey(in Type key)
        => _enumerables.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<Type, EnumerableBundle>.TryGetCache(in Type key, out EnumerableBundle cached)
        => _enumerables.TryGetValue(key, out cached);
    /// <inheritdoc />
    void IStore<Type, EnumerableBundle>.Set(in Type key, EnumerableBundle value)
        => _enumerables[key] = value;
    #endregion
    #region ICacher<Type, CollectionBundle>
    /// <inheritdoc />
    bool ICacher<Type, CollectionBundle>.ContainsKey(in Type key)
        => _collections.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<Type, CollectionBundle>.TryGetCache(in Type key, out CollectionBundle cached)
        => _collections.TryGetValue(key, out cached);
    /// <inheritdoc />
    void IStore<Type, CollectionBundle>.Set(in Type key, CollectionBundle value)
        => _collections[key] = value;
    #endregion
    #region ICacher<Type, ListBundle>
    /// <inheritdoc />
    bool ICacher<Type, ListBundle>.ContainsKey(in Type key)
        => _lists.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<Type, ListBundle>.TryGetCache(in Type key, out ListBundle cached)
        => _lists.TryGetValue(key, out cached);
    /// <inheritdoc />
    void IStore<Type, ListBundle>.Set(in Type key, ListBundle value)
        => _lists[key] = value;
    #endregion
    #region ICacher<Type, DictionaryBundle>
    /// <inheritdoc />
    bool ICacher<Type, DictionaryBundle>.ContainsKey(in Type key)
        => _dictionaries.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<Type, DictionaryBundle>.TryGetCache(in Type key, out DictionaryBundle cached)
        => _dictionaries.TryGetValue(key, out cached);
    /// <inheritdoc />
    void IStore<Type, DictionaryBundle>.Set(in Type key, DictionaryBundle value)
        => _dictionaries[key] = value;
    #endregion
    /// <summary>
    /// 集合访问者缓存
    /// </summary>
    internal VisitorCacher VisitorCacher
        => _visitorCacher;
    /// <summary>
    /// 索引读取器缓存
    /// </summary>
    internal ReadIndexCacher ReadIndexCacher
        => _readIndexCacher;
    /// <summary>
    /// 集合访问者缓存
    /// </summary>
    /// <returns></returns>
    internal IndexVisitorCacher IndexVisitorCacher
        => _indexVisitorCacher;
    /// <summary>
    /// 获取集合数量缓存
    /// </summary>
    internal CountCacher CountCacher 
        => _countCacher;
    /// <summary>
    /// 获取集合元素保持器缓存
    /// </summary>
    internal SaveCacher SaveCacher 
        => _saveCacher;
    /// <summary>
    /// 迭代器缓存
    /// </summary>
    internal EnumerableCacher EnumerableCacher
        => _enumerableCacher;
    /// <summary>
    /// 集合成员缓存
    /// </summary>
    internal CollectionCacher CollectionCacher
        => _collectionCacher;
    /// <summary>
    /// 列表成员缓存
    /// </summary>
    internal ListCacher ListCacher
        => _listCacher;
    /// <summary>
    /// 字典成员缓存
    /// </summary>
    internal DictionaryCacher DictionaryCacher
        => _dictionaryCacher;
    /// <summary>
    /// 实例
    /// </summary>
    public static CollectionContainer Instance
        => Inner.Instance;
    #region 配置
    /// <summary>
    /// 全局启用集合功能(转化和复制)
    /// </summary>
    /// <returns></returns>
    public static void GlobalUseCollection()
        => Mapper.GlobalConfigure(mapper => mapper.UseCollection());
    /// <summary>
    /// 配置
    /// </summary>
    private static Action<CollectionContainerOptions> _options = null;
    /// <summary>
    /// 配置(在第一次调用Instance之前配置才能生效)
    /// </summary>
    /// <param name="options"></param>
    public static void Configure(Action<CollectionContainerOptions> options)
    {
        if (options is null)
            return;
        if (_options is null)
            _options = options;
        else
            _options += options;
    }
    #endregion
    /// <summary>
    /// 内部延迟加载
    /// </summary>
    sealed class Inner
    {
        /// <summary>
        /// 集合容器
        /// </summary>
        internal static readonly CollectionContainer Instance = Create();
        /// <summary>
        /// 构造
        /// </summary>
        /// <returns></returns>
        private static CollectionContainer Create()
        {
            var options = new CollectionContainerOptions();
            _options?.Invoke(options);
            return new CollectionContainer(options);
        }
    }
}
