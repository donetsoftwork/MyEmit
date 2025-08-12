using PocoEmit.Collections;
using PocoEmit.Collections.Cachers;
using PocoEmit.Collections.Visitors;
using PocoEmit.Configuration;
using PocoEmit.Indexs;
using System;
using System.Collections.Concurrent;

#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
using System.Collections.Generic;
using System.Collections.Frozen;
#endif

namespace PocoEmit;

/// <summary>
/// 集合容器
/// </summary>
public sealed class CollectionContainer
    : ICacher<Type, IEmitCollectionCounter>
    , ICacher<Type, ICollectionVisitor>
    , ICacher<Type, IEmitIndexMemberReader>
    , ICacher<Type, IElementIndexVisitor>
{
    /// <summary>
    /// 集合容器
    /// </summary>
    private CollectionContainer(CollectionContainerOptions options)
    {
        var concurrencyLevel = options.ConcurrencyLevel;
        _counters = new ConcurrentDictionary<Type, IEmitCollectionCounter>(concurrencyLevel, options.CounterCapacity);
        _visitors = new ConcurrentDictionary<Type, ICollectionVisitor>(concurrencyLevel, options.VisitorCapacity);
        _readIndexs = new ConcurrentDictionary<Type, IEmitIndexMemberReader>(concurrencyLevel, options.ReadIndexCapacity);
        _indexVisitors = new ConcurrentDictionary<Type, IElementIndexVisitor>(concurrencyLevel, options.ReadIndexCapacity);
        _countCacher = new(this);
        _visitorCacher = new(this);
        _readIndexCacher = new(this);
        _indexVisitorCacher = new(this);
    }
    #region 配置
    private readonly CountCacher _countCacher;
    private readonly VisitorCacher _visitorCacher;
    private readonly ReadIndexCacher _readIndexCacher;
    private readonly IndexVisitorCacher _indexVisitorCacher;
#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    /// <summary>
    /// 集合数量缓存
    /// </summary>
    private IDictionary<Type, IEmitCollectionCounter> _counters;
    /// <summary>
    /// 集合访问者缓存
    /// </summary>
    private IDictionary<Type, ICollectionVisitor> _visitors;
    /// <summary>
    /// 索引读取器缓存
    /// </summary>
    private IDictionary<Type, IEmitIndexMemberReader> _readIndexs;
    /// <summary>
    /// 索引访问者缓存(主要用于字典遍历)
    /// </summary>
    private IDictionary<Type, IElementIndexVisitor> _indexVisitors;
#else
    /// <summary>
    /// 集合数量缓存
    /// </summary>
    private ConcurrentDictionary<Type, IEmitCollectionCounter> _counters;
    /// <summary>
    /// 集合访问者缓存
    /// </summary>
    private ConcurrentDictionary<Type, ICollectionVisitor> _visitors;
    /// <summary>
    /// 索引读取器缓存
    /// </summary>
    private ConcurrentDictionary<Type, IEmitIndexMemberReader> _readIndexs;
        /// <summary>
    /// 索引访问者缓存(主要用于字典遍历)
    /// </summary>
    private ConcurrentDictionary<Type, IElementIndexVisitor> _indexVisitors;
#endif
    #endregion
    #region ICacher<Type, IEmitCollectionCounter>
    /// <inheritdoc />
    bool ICacher<Type, IEmitCollectionCounter>.ContainsKey(Type key)
        => _counters.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<Type, IEmitCollectionCounter>.TryGetValue(Type key, out IEmitCollectionCounter value)
        => _counters.TryGetValue(key, out value);
    /// <inheritdoc />
    void IStore<Type, IEmitCollectionCounter>.Set(Type key, IEmitCollectionCounter value)
        => _counters[key] = value;
    #endregion
    #region ICacher<Type, ICollectionVisitor>
    /// <inheritdoc />
    bool ICacher<Type, ICollectionVisitor>.ContainsKey(Type key)
        => _visitors.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<Type, ICollectionVisitor>.TryGetValue(Type key, out ICollectionVisitor value)
        => _visitors.TryGetValue(key, out value);
    /// <inheritdoc />
    void IStore<Type, ICollectionVisitor>.Set(Type key, ICollectionVisitor value)
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
    /// <summary>
    /// 获取集合数量
    /// </summary>
    /// <param name="collectionType"></param>
    /// <returns></returns>
    public IEmitCollectionCounter GetCounter(Type collectionType)
        => _countCacher.Get(collectionType);
    /// <summary>
    /// 获取集合访问者
    /// </summary>
    /// <param name="collectionType"></param>
    /// <returns></returns>
    public ICollectionVisitor GetVisitor(Type collectionType)
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
    }
#endif
    #region 配置
    /// <summary>
    /// 全局启用集合功能(转化和复制)
    /// </summary>
    /// <returns></returns>
    public static void GlobalUseCollections()
        => Mapper.GlobalConfigure(mapper => mapper.UseCollections());
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


    sealed class Inner
    {
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
