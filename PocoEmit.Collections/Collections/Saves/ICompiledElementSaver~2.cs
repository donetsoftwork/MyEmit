using System;

namespace PocoEmit.Collections.Saves;

/// <summary>
/// 已编集合元素保存器
/// </summary>
/// <typeparam name="TCollection"></typeparam>
/// <typeparam name="Element"></typeparam>
public interface ICompiledElementSaver<TCollection, Element>
    : ICollectionSaver<TCollection, Element>
    , IEmitElementSaver
{
    /// <summary>
    /// 保存子元素委托
    /// </summary>
    Action<TCollection, Element> SaveAction { get; }
}
