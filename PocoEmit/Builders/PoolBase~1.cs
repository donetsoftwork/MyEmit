using System;
using System.Collections.Concurrent;

namespace PocoEmit.Builders;

/// <summary>
/// 池基类
/// </summary>
/// <typeparam name="TResource"></typeparam>
public abstract class PoolBase<TResource>
    : IPool<TResource>
    where TResource : class
{
    /// <summary>
    /// 池基类
    /// </summary>
    public PoolBase()
        : this(0, Environment.ProcessorCount)
    { 
    }
    /// <summary>
    /// 池基类
    /// </summary>
    /// <param name="initialSize"></param>
    /// <param name="maxSize"></param>
    public PoolBase(int initialSize, int maxSize)
    {
        _maxSize = maxSize;
        Initialize(initialSize);
    }
    #region 配置
    private readonly int _maxSize;
    /// <summary>
    /// 资源池
    /// </summary>
    private readonly ConcurrentBag<TResource> _pool = [];
    /// <summary>
    /// 最大容量
    /// </summary>
    public int MaxSize 
        => _maxSize;
    #endregion
    /// <inheritdoc />
    public TResource Get()
    {
        if (_pool.TryTake(out var resource))
            return resource;
        _pool.Add(resource = CreateNew());
        return resource;
    }
    /// <inheritdoc />
    public void Return(TResource resource)
    {
        if (Clean(ref resource))
            _pool.Add(resource);
    }
    /// <summary>
    /// 构造新对象
    /// </summary>
    /// <returns></returns>
    protected abstract TResource CreateNew();
    /// <summary>
    /// 初始化对象池
    /// </summary>
    /// <param name="initialSize"></param>
    protected virtual void Initialize(int initialSize) 
    {
        for (var i = 0; i < initialSize; i++)
            _pool.Add(CreateNew());
    }
    /// <summary>
    /// 检查是否超过最大容量
    /// </summary>
    /// <returns></returns>
    protected bool CheckMaxSize()
        => _pool.Count < _maxSize;
    /// <summary>
    /// 清理对象(及判断对象是否能重入)
    /// </summary>
    /// <param name="resource"></param>
    protected virtual bool Clean(ref TResource resource)
        => CheckMaxSize();
}
