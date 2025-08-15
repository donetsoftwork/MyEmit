using System;

namespace PocoEmit.Collections.Counters;

/// <summary>
/// 已编译元素数量获取器
/// </summary>
/// <typeparam name="TCollection"></typeparam>
public interface ICompiledCounter<TCollection>
    : IEmitElementCounter
    , ICounter<TCollection>
{
    /// <summary>
    /// 获取数量委托
    /// </summary>
    Func<TCollection, int> CountFunc { get; }
}
