using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Counters;

/// <summary>
/// 已编译数量获取器
/// </summary>
/// <typeparam name="TCollection"></typeparam>
/// <param name="inner"></param>
/// <param name="countFunc"></param>
public class CompiledCounter<TCollection>(IEmitElementCounter inner, Func<TCollection, int> countFunc)
    : ICompiledCounter<TCollection>
{
    #region 配置
    private readonly IEmitElementCounter _inner = inner;
    private readonly Func<TCollection, int> _countFunc = countFunc;
    /// <inheritdoc />
    public Type CollectionType 
        => _inner.CollectionType;
    /// <inheritdoc />
    public Type ElementType 
        => _inner.ElementType;
    /// <inheritdoc />
    public Func<TCollection, int> CountFunc
        => _countFunc;
    /// <inheritdoc />
    bool IEmitCounter.CountByProperty
        => _inner.CountByProperty;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => true;
    #endregion
    /// <inheritdoc />
    Expression IEmitCounter.Count(Expression collection)
        => _inner.Count(collection);
    /// <inheritdoc />
    public int Count(TCollection collection)
        => _countFunc(collection);
}
