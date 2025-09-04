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

#if NET8_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
using System.Collections.Generic;
using System.Collections.Frozen;
#endif

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
#if NET8_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    /// <summary>
    /// 集合数量缓存
    /// </summary>
    private IDictionary<PairTypeKey, IEmitElementCounter> _counters;
    /// <summary>
    /// 集合访问者缓存
    /// </summary>
    private IDictionary<Type, IEmitElementVisitor> _visitors;
    /// <summary>
    /// 索引读取器缓存
    /// </summary>
    private IDictionary<Type, IEmitIndexMemberReader> _readIndexs;
    /// <summary>
    /// 索引访问者缓存(主要用于字典遍历)
    /// </summary>
    private IDictionary<Type, IElementIndexVisitor> _indexVisitors;
    /// <summary>
    /// 元素保存器
    /// </summary>
    private IDictionary<PairTypeKey, IEmitElementSaver> _savers;
    /// <summary>
    /// 迭代类成员
    /// </summary>
    private IDictionary<Type, EnumerableBundle> _enumerables;
    /// <summary>
    /// 集合类成员
    /// </summary>
    private IDictionary<Type, CollectionBundle> _collections;
    /// <summary>
    /// 列表类成员
    /// </summary>
    private IDictionary<Type, ListBundle> _lists;
    /// <summary>
    /// 字典类成员
    /// </summary>
    private IDictionary<Type, DictionaryBundle> _dictionaries;
#else
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

#endif
    #endregion
    #region ICacher<Type, IEmitElementCounter>
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IEmitElementCounter>.ContainsKey(PairTypeKey key)
        => _counters.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IEmitElementCounter>.TryGetValue(PairTypeKey key, out IEmitElementCounter value)
        => _counters.TryGetValue(key, out value);
    /// <inheritdoc />
    void IStore<PairTypeKey, IEmitElementCounter>.Set(PairTypeKey key, IEmitElementCounter value)
        => _counters[key] = value;
    #endregion
    #region ICacher<Type, ICollectionVisitor>
    /// <inheritdoc />
    bool ICacher<Type, IEmitElementVisitor>.ContainsKey(Type key)
        => _visitors.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<Type, IEmitElementVisitor>.TryGetValue(Type key, out IEmitElementVisitor value)
        => _visitors.TryGetValue(key, out value);
    /// <inheritdoc />
    void IStore<Type, IEmitElementVisitor>.Set(Type key, IEmitElementVisitor value)
        => _visitors[key] = value;
    #endregion
    #region ICacher<Type, IEmitIndexMemberReader>
    /// <inheritdoc />
    bool ICacher<Type, IEmitIndexMemberReader>.ContainsKey(Type key)
        => _readIndexs.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<Type, IEmitIndexMemberReader>.TryGetValue(Type key, out IEmitIndexMemberReader value)
        => _readIndexs.TryGetValue(key, out value);
    /// <inheritdoc />
    void IStore<Type, IEmitIndexMemberReader>.Set(Type key, IEmitIndexMemberReader value)
        => _readIndexs[key] = value;
    #endregion
    #region ICacher<Type, IElementIndexVisitor>
    /// <inheritdoc />
    bool ICacher<Type, IElementIndexVisitor>.ContainsKey(Type key)
        => _indexVisitors.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<Type, IElementIndexVisitor>.TryGetValue(Type key, out IElementIndexVisitor value)
        => _indexVisitors.TryGetValue(key, out value);
    /// <inheritdoc />
    void IStore<Type, IElementIndexVisitor>.Set(Type key, IElementIndexVisitor value)
        => _indexVisitors[key] = value;
    #endregion
    #region ICacher<Type, IEmitElementSaver>
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IEmitElementSaver>.ContainsKey(PairTypeKey key)
        => _savers.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<PairTypeKey, IEmitElementSaver>.TryGetValue(PairTypeKey key, out IEmitElementSaver value)
        => _savers.TryGetValue(key, out value);
    /// <inheritdoc />
    void IStore<PairTypeKey, IEmitElementSaver>.Set(PairTypeKey key, IEmitElementSaver value)
        => _savers[key] = value;
    #endregion
    #region ICacher<Type, EnumerableBundle>
    /// <inheritdoc />
    bool ICacher<Type, EnumerableBundle>.ContainsKey(Type key)
        => _enumerables.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<Type, EnumerableBundle>.TryGetValue(Type key, out EnumerableBundle value)
        => _enumerables.TryGetValue(key, out value);
    /// <inheritdoc />
    void IStore<Type, EnumerableBundle>.Set(Type key, EnumerableBundle value)
        => _enumerables[key] = value;
    #endregion
    #region ICacher<Type, CollectionBundle>
    /// <inheritdoc />
    bool ICacher<Type, CollectionBundle>.ContainsKey(Type key)
        => _collections.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<Type, CollectionBundle>.TryGetValue(Type key, out CollectionBundle value)
        => _collections.TryGetValue(key, out value);
    /// <inheritdoc />
    void IStore<Type, CollectionBundle>.Set(Type key, CollectionBundle value)
        => _collections[key] = value;
    #endregion
    #region ICacher<Type, ListBundle>
    /// <inheritdoc />
    bool ICacher<Type, ListBundle>.ContainsKey(Type key)
        => _lists.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<Type, ListBundle>.TryGetValue(Type key, out ListBundle value)
        => _lists.TryGetValue(key, out value);
    /// <inheritdoc />
    void IStore<Type, ListBundle>.Set(Type key, ListBundle value)
        => _lists[key] = value;
    #endregion
    #region ICacher<Type, DictionaryBundle>
    /// <inheritdoc />
    bool ICacher<Type, DictionaryBundle>.ContainsKey(Type key)
        => _dictionaries.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<Type, DictionaryBundle>.TryGetValue(Type key, out DictionaryBundle value)
        => _dictionaries.TryGetValue(key, out value);
    /// <inheritdoc />
    void IStore<Type, DictionaryBundle>.Set(Type key, DictionaryBundle value)
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
#if NET8_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    /// <summary>
    /// 设置为不可变
    /// </summary>
    public void ToFrozen()
    {
        _counters = _counters.ToFrozenDictionary();
        _visitors = _visitors.ToFrozenDictionary();
        _readIndexs = _readIndexs.ToFrozenDictionary();
        _indexVisitors = _indexVisitors.ToFrozenDictionary();
        _savers = _savers.ToFrozenDictionary();
        _enumerables = _enumerables.ToFrozenDictionary();
        _dictionaries = _dictionaries.ToFrozenDictionary();
    }
#endif
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
