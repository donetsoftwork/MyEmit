namespace PocoEmit.Collections;

/// <summary>
/// 集合元素保存器
/// </summary>
/// <typeparam name="TCollection"></typeparam>
/// <typeparam name="TElement"></typeparam>
public interface ICollectionSaver<TCollection, TElement>
{
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="item"></param>
    void Add(TCollection collection, TElement item);
}
