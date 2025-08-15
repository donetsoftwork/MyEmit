using PocoEmit.Collections;
using PocoEmit.Collections.Cachers;
using PocoEmit.Collections.Counters;
using PocoEmit.Collections.Visitors;
using PocoEmit.Configuration;
using PocoEmit.Indexs;
using System;
using System.Collections.Concurrent;
using PocoEmit.Collections.Saves;

#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
using System.Collections.Generic;
using System.Collections.Frozen;
#endif

namespace PocoEmit;

/// <summary>
/// 集合容器
/// </summary>
public sealed class CollectionContainer
    : ICacher<PairTypeKey, IEmitElementCounter>
    , ICacher<Type, IEmitElementVisitor>
    , ICacher<Type, IEmitIndexMemberReader>
    , ICacher<Type, IElementIndexVisitor>
    , ICacher<PairTypeKey, IEmitElementSaver>
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
        _countCacher = new(this);
        _visitorCacher = new(this);
        _readIndexCacher = new(this);
        _indexVisitorCacher = new(this);
        _saveCacher = new(this);
    }
    #region 配置
    private readonly CountCacher _countCacher;
    private readonly VisitorCacher _visitorCacher;
    private readonly ReadIndexCacher _readIndexCacher;
    private readonly IndexVisitorCacher _indexVisitorCacher;
    private readonly SaveCacher _saveCacher;
#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
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
    /// <summary>
    /// 获取集合访问者
    /// </summary>
    /// <param name="collectionType"></param>
    /// <returns></returns>
    public IEmitElementVisitor GetVisitor(Type collectionType)
        => _visitorCacher.Get(collectionType);
    /// <summary>
    /// 获取索引读取器
    /// </summary>
    /// <param name="collectionType"></param>
    /// <returns></returns>
    public IEmitIndexMemberReader GetIndexReader(Type collectionType)
        => _readIndexCacher.Get(collectionType);
    /// <summary>
    /// 获取集合访问者
    /// </summary>
    /// <param name="collectionType"></param>
    /// <returns></returns>
    public IElementIndexVisitor GetIndexVisitor(Type collectionType)
        => _indexVisitorCacher.Get(collectionType);
    /// <summary>
    /// 获取集合数量缓存
    /// </summary>
    public CacheBase<PairTypeKey, IEmitElementCounter> CountCacher 
        => _countCacher;
    /// <summary>
    /// 获取集合元素保持器缓存
    /// </summary>
    public CacheBase<PairTypeKey, IEmitElementSaver> SaveCacher 
        => _saveCacher;
    /// <summary>
    /// 实例
    /// </summary>
    public static CollectionContainer Instance
        => Inner.Instance;
#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
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
