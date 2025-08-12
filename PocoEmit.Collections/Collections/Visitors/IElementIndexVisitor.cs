using System;

namespace PocoEmit.Collections.Visitors;

/// <summary>
/// 元素索引访问者
/// </summary>
public interface IElementIndexVisitor : IIndexVisitor
{
    /// <summary>
    /// 主键类型
    /// </summary>
    Type KeyType { get; }
    /// <summary>
    /// 元素类型
    /// </summary>
    Type ElementType { get; }
}
