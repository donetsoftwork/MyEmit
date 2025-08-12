using PocoEmit.Configuration;
using System;

namespace PocoEmit.Collections;

/// <summary>
/// 元素数量获取器
/// </summary>
public interface IEmitCollectionCounter
    : IEmitCounter, ICompileInfo
{
    /// <summary>
    /// 集合类型
    /// </summary>
    Type CollectionType { get; }
}
