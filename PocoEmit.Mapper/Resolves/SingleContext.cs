using PocoEmit.Builders;
using System;
using System.Collections.Generic;

namespace PocoEmit.Resolves;

/// <summary>
/// 单一类型转化上下文
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
public class SingleContext<TSource, TDest>(IPool<SingleContext<TSource, TDest>> pool)
    : IConvertContext
{
    #region 配置
    private readonly IPool<SingleContext<TSource, TDest>> _pool = pool;
    private readonly Dictionary<TSource, TDest> _cacher = [];
    private bool _hasCached = false;
    #endregion
    #region IConvertContext
    /// <inheritdoc />
    public bool TryGetCache<S, T>(S s, out T t)
    {
        if (_hasCached && s is TSource source)
        {
            if (_cacher.TryGetValue(source, out var cached))
            {
                if (cached is T t1)
                    t = t1;
                else
                    t = default;
                return true;
            }
        }
        t = default;
        return false;
    }
    /// <inheritdoc />
    void IConvertContext.SetCache<S, T>(S s, T t)
    {
        if(s is TSource source && t is TDest dest)
        {
            _cacher[source] = dest;
            _hasCached = true;
        }
    }
    #endregion
    /// <summary>
    /// 清空
    /// </summary>
    public void Clear()
    {
        _cacher.Clear();
        _hasCached = false;
    }
    /// <summary>
    /// 收回
    /// </summary>
    public void Dispose()
        => _pool.Return(this);
    #region Pool
    /// <summary>
    /// 池化管理器
    /// </summary>
    public static readonly IPool<SingleContext<TSource, TDest>> Pool = new PoolManager();
    /// <summary>
    /// 转换上下文管理器
    /// </summary>
    class PoolManager()
        : PoolBase<SingleContext<TSource, TDest>>(1, Environment.ProcessorCount)
    {
        /// <inheritdoc />
        protected override SingleContext<TSource, TDest> CreateNew()
            => new(this);
        /// <inheritdoc />
        protected override bool Clean(ref SingleContext<TSource, TDest> resource)
        {
            if (CheckMaxSize())
            {
                resource.Clear();
                return true;
            }
            return false;
        }
    }
    #endregion
}
