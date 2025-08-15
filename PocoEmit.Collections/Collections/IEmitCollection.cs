using System;

namespace PocoEmit.Collections;

/// <summary>
/// Emit集合
/// </summary>
public interface IEmitCollection
{
    /// <summary>
    /// 集合类型
    /// </summary>
    public Type CollectionType { get; }
    /// <summary>
    /// 子元素类型
    /// </summary>
    public Type ElementType { get; }
}
