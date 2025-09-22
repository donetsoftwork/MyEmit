using PocoEmit.Builders;
using PocoEmit.Converters;
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
    /// <summary>
    /// 回收池
    /// </summary>
    public IPool<SingleContext<TSource, TDest>> Pool
        => _pool;
    #endregion
    #region IConvertContext
    /// <inheritdoc />
    public T Convert<S, T>(IContextConverter converter, S s)
    {
        if (s is TSource source)
        {
            if (_cacher.TryGetValue(source, out var cached))
            {
                if (cached is T t)
                    return t;
            }
            else
            {
                var t = converter.Convert<S, T>(this, s);
                if (t is TDest dest)
                    _cacher[source] = dest;
                return t;
            }
        }
        return default;
    }
    /// <inheritdoc />
    void IConvertContext.SetCache<S, T>(S s, T t)
    {
        if(s is TSource source && t is TDest dest)
            _cacher[source] = dest;
    }
    #endregion
    /// <summary>
    /// 清空
    /// </summary>
    public void Clear()
        => _cacher.Clear();
    /// <summary>
    /// 收回
    /// </summary>
    public void Dispose()
        => _pool.Return(this);
    /// <summary>
    /// 转换上下文管理器
    /// </summary>
    internal class Manager()
        : PoolBase<SingleContext<TSource, TDest>>(Environment.ProcessorCount, Environment.ProcessorCount << 2)
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
        /// <summary>
        /// 单例
        /// </summary>
        public static readonly Manager Default = new();
    }
}
